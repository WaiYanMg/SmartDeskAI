using Microsoft.EntityFrameworkCore;
using Smart_Desk_AI.Data;
using Smart_Desk_AI.Models;

namespace Smart_Desk_AI.Services
{
    public class ReviewService
    {
        private readonly AppDbContext _db;

        public ReviewService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> NeedsHumanReview(
            SubmissionRequest request,
            List<AgentDecision> decisions)
        {
            var avgConfidence = decisions.Average(d => d.ConfidenceScore);
            var hasEscalation = decisions.Any(d => d.Decision == "ESCALATE");

            // Always human review if AI is unsure
            if (avgConfidence < 85) return true;

            // Always human review if any agent escalated
            if (hasEscalation) return true;

            // Check specific request type rules
            switch (request.RequestType.ToLower())
            {
                case "leave":
                    // Long leave always needs human
                    if (request.Fields != null &&
                        request.Fields.Contains("days") &&
                        ExtractDays(request.Fields) > 5)
                        return true;
                    break;

                case "expense":
                case "claim":
                    // High value claims always need human
                    if (request.Fields != null &&
                        request.Fields.Contains("amount") &&
                        ExtractAmount(request.Fields) > 500)
                        return true;
                    break;

                case "cv":
                case "recruitment":
                    // CV reviews always need human
                    return true;
            }

            return false;
        }

        public int GetPriority(
            SubmissionRequest request,
            List<AgentDecision> decisions)
        {
            var avgConfidence = decisions.Average(d => d.ConfidenceScore);

            // Priority 1 - URGENT
            if (request.RequestType.ToLower() == "medical" ||
                request.RequestType.ToLower() == "legal" ||
                ExtractAmountSafe(request.Fields) > 1000)
                return 1;

            // Priority 2 - HIGH
            if (avgConfidence < 60 ||
                decisions.Any(d => d.Decision == "ESCALATE") ||
                ExtractAmountSafe(request.Fields) > 500)
                return 2;

            // Priority 3 - MEDIUM (default)
            if (avgConfidence < 85)
                return 3;

            // Priority 4 - LOW (routine spot check)
            return 4;
        }

        public async Task AddToReviewQueue(
            RequestRecord record,
            List<AgentDecision> decisions,
            int priority)
        {
            var avgConfidence = (int)decisions.Average(d => d.ConfidenceScore);
            var aiRecommendation = decisions.Any(d => d.Decision == "DENIED")
                ? "DENIED" : decisions.Any(d => d.Decision == "ESCALATE")
                ? "ESCALATE" : "APPROVED";

            var queueItem = new ReviewQueue
            {
                RequestId = record.Id,
                Priority = priority,
                Status = "PENDING",
                AiRecommendation = aiRecommendation,
                AiConfidence = avgConfidence,
                CreatedAt = DateTime.UtcNow
            };

            _db.ReviewQueues.Add(queueItem);

            // Create notification for manager
            var notification = new Notification
            {
                Title = GetNotificationTitle(priority, record.RequestType),
                Message = $"New {record.RequestType} request requires your review. " +
                         $"AI recommends: {aiRecommendation} " +
                         $"(confidence: {avgConfidence}%)",
                Type = "review_required",
                IsRead = false,
                RequestId = record.Id,
                CreatedAt = DateTime.UtcNow
            };

            _db.Notifications.Add(notification);
            await _db.SaveChangesAsync();
        }

        private string GetNotificationTitle(int priority, string requestType)
        {
            return priority switch
            {
                1 => $"🔴 URGENT: {requestType} request needs immediate review",
                2 => $"🟠 HIGH: {requestType} request needs review",
                3 => $"🟡 MEDIUM: {requestType} request pending review",
                _ => $"🟢 LOW: {requestType} request for spot check"
            };
        }

        private int ExtractDays(string? fields)
        {
            if (string.IsNullOrEmpty(fields)) return 0;
            if (fields.Contains("days\":"))
            {
                var start = fields.IndexOf("days\":") + 6;
                var end = fields.IndexOf(",", start);
                if (end == -1) end = fields.IndexOf("}", start);
                if (int.TryParse(fields.Substring(start, end - start)
                    .Trim().Trim('"'), out int days))
                    return days;
            }
            return 0;
        }

        private decimal ExtractAmount(string? fields)
        {
            if (string.IsNullOrEmpty(fields)) return 0;
            var cleaned = fields.Replace("€", "")
                               .Replace("£", "")
                               .Replace("$", "");
            if (cleaned.Contains("amount\":"))
            {
                var start = cleaned.IndexOf("amount\":") + 8;
                var end = cleaned.IndexOf(",", start);
                if (end == -1) end = cleaned.IndexOf("}", start);
                if (decimal.TryParse(cleaned.Substring(start, end - start)
                    .Trim().Trim('"'), out decimal amount))
                    return amount;
            }
            return 0;
        }

        private decimal ExtractAmountSafe(string? fields)
        {
            try { return ExtractAmount(fields); }
            catch { return 0; }
        }
    }
}