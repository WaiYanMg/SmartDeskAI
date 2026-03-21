namespace Smart_Desk_AI.Models
{
    public class User
    {
        public int Id { get; set; }
        public int? BusinessId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "employee";
        public string? Department { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Business? Business { get; set; }
    }
}