namespace Smart_Desk_AI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Business? Business { get; set; }
    }
}