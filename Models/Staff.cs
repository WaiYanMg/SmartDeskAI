namespace Smart_Desk_AI.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Speciality { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Business? Business { get; set; }
    }
}