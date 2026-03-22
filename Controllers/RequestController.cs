using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Desk_AI.Agents;
using Smart_Desk_AI.Data;
using Smart_Desk_AI.Models;
using Smart_Desk_AI.Services;
using System.Text.Json;

namespace Smart_Desk_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly OrchestratorAgent _orchestrator;
        private readonly DocumentService _documentService;
        private readonly IConfiguration _config;
        private readonly AppDbContext _db;
        private readonly ReviewService _reviewService;

        public RequestController(
            OrchestratorAgent orchestrator,
            DocumentService documentService,
            IConfiguration config,
            AppDbContext db,
            ReviewService reviewService)
        {
            _orchestrator = orchestrator;
            _documentService = documentService;
            _config = config;
            _db = db;
            _reviewService = reviewService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit(
            [FromForm] SubmissionRequest request)
        {
            // Extract text from uploaded document if provided
            if (request.Document != null)
            {
                var extractedText = await _documentService
                    .ExtractFromUploadAsync(request.Document);
                request.Description += $"\n\nDocument Content:\n{extractedText}";
            }

            // Process through orchestrator
            var decisions = await _orchestrator.ProcessAsync(request);

            // Check if needs human review
            var needsHuman = await _reviewService
                .NeedsHumanReview(request, decisions);

            var priority = _reviewService.GetPriority(request, decisions);

            var finalOutcome = needsHuman
    ? "PENDING_REVIEW"
    : decisions.Any(d => d.Decision == "CONFIRMED")
    ? "CONFIRMED"
    : decisions.All(d => d.Decision == "APPROVED")
    ? "APPROVED"
    : decisions.Any(d => d.Decision == "DENIED")
    ? "DENIED" : "ESCALATE";

            // Save to database
            var record = new RequestRecord
            {
                RequestType = request.RequestType,
                Description = request.Description,
                Fields = request.Fields,
                FinalOutcome = finalOutcome,
                AiDecisions = JsonSerializer.Serialize(decisions),
                CreatedAt = DateTime.UtcNow
            };

            _db.Requests.Add(record);
            await _db.SaveChangesAsync();

            // Add to review queue if needed
            if (needsHuman)
            {
                await _reviewService.AddToReviewQueue(
                    record, decisions, priority);
            }

            return Ok(new
            {
                requestId = record.Id,
                requestType = request.RequestType,
                finalOutcome = finalOutcome,
                needsHumanReview = needsHuman,
                priority = needsHuman ? GetPriorityLabel(priority) : null,
                message = needsHuman
                    ? "Your request has been sent for human review. You will be notified soon."
                    : "Your request has been processed automatically.",
                decisions = decisions,
                timestamp = record.CreatedAt
            });
        }

        [HttpGet("history")]
public async Task<IActionResult> History()
{
    var records = await _db.Requests
        .OrderByDescending(r => r.CreatedAt)
        .Take(20)
        .Select(r => new
        {
            r.Id,
            r.RequestType,
            r.Description,
            r.Fields,
            r.FinalOutcome,
            r.AiDecisions,
            r.CreatedAt,
            // Get human decision from review queue
            HumanDecision = _db.ReviewQueues
                .Where(q => q.RequestId == r.Id && q.Status == "REVIEWED")
                .Select(q => q.HumanDecision)
                .FirstOrDefault(),
            HumanNotes = _db.ReviewQueues
                .Where(q => q.RequestId == r.Id && q.Status == "REVIEWED")
                .Select(q => q.HumanNotes)
                .FirstOrDefault()
        })
        .ToListAsync();

    return Ok(records);
}

        [HttpGet("queue")]
        public async Task<IActionResult> ReviewQueue()
        {
            var queue = await _db.ReviewQueues
                .Include(r => r.Request)
                .Where(r => r.Status == "PENDING")
                .OrderBy(r => r.Priority)
                .ThenBy(r => r.CreatedAt)
                .ToListAsync();
            return Ok(queue);
        }

        [HttpPost("queue/{id}/decide")]
        public async Task<IActionResult> HumanDecide(
            int id, [FromBody] HumanDecisionRequest decision)
        {
            var queueItem = await _db.ReviewQueues
                .Include(r => r.Request)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (queueItem == null)
                return NotFound("Review item not found");

            // Update queue item
            queueItem.HumanDecision = decision.Decision;
            queueItem.HumanNotes = decision.Notes;
            queueItem.Status = "REVIEWED";
            queueItem.ReviewedAt = DateTime.UtcNow;

            // Update original request
            if (queueItem.Request != null)
            {
                queueItem.Request.FinalOutcome = decision.Decision;
            }

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = $"Request {decision.Decision} by human reviewer",
                requestId = queueItem.RequestId,
                decision = decision.Decision,
                notes = decision.Notes,
                reviewedAt = queueItem.ReviewedAt
            });
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var notifications = await _db.Notifications
                .OrderByDescending(n => n.CreatedAt)
                .Take(20)
                .ToListAsync();
            return Ok(notifications);
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "SmartDesk AI is running!" });
        }

        [HttpGet("test-ai")]
        public async Task<IActionResult> TestAI(
            [FromServices] OpenAIService openAI)
        {
            try
            {
                var response = await openAI.ChatAsync(
                    "You are a helpful assistant.",
                    "Say hello in one word");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("test-db")]
        public async Task<IActionResult> TestDB()
        {
            try
            {
                var canConnect = await _db.Database.CanConnectAsync();
                return Ok(new
                {
                    connected = canConnect,
                    message = canConnect
                        ? "Database connected successfully!"
                        : "Connection failed!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    connected = false,
                    message = ex.Message
                });
            }
        }

        private string GetPriorityLabel(int priority) => priority switch
        {
            1 => "🔴 URGENT",
            2 => "🟠 HIGH",
            3 => "🟡 MEDIUM",
            _ => "🟢 LOW"
        };
    }
}