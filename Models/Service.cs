namespace Smart_Desk_AI.Models
{
    public class Service
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Business? Business { get; set; }
    }
}