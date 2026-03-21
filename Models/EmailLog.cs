namespace Smart_Desk_AI.Models
{
    public class EmailLog
    {
        public int Id { get; set; }
        public string ToEmail { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string Status { get; set; } = "PENDING";
        public DateTime? SentAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}