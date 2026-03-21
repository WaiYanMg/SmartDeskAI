using System.Text.Json;

namespace Smart_Desk_AI.Models
{
    public class RequestRecord
    {
        public int Id { get; set; }
        public int? BusinessId { get; set; }
        public int? EmployeeId { get; set; }
        public string RequestType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Fields { get; set; }
        public string? FinalOutcome { get; set; }
        public string? AiDecisions { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Business? Business { get; set; }
        public Employee? Employee { get; set; }
    }
}
