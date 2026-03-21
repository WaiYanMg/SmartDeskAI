namespace Smart_Desk_AI.Models
{
    public class ReviewQueue
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int Priority { get; set; } = 3;
        public string Status { get; set; } = "PENDING";
        public string? AiRecommendation { get; set; }
        public int AiConfidence { get; set; }
        public int? AssignedTo { get; set; }
        public string? HumanDecision { get; set; }
        public string? HumanNotes { get; set; }
        public DateTime? NotifiedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public RequestRecord? Request { get; set; }
    }
}