namespace Smart_Desk_AI.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Message { get; set; }
        public string? Type { get; set; }
        public bool IsRead { get; set; } = false;
        public int? RequestId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public User? User { get; set; }
        public RequestRecord? Request { get; set; }
    }
}