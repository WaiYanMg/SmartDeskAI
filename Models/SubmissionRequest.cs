namespace Smart_Desk_AI.Models
{
    public class SubmissionRequest
    {
        public string RequestType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile? Document { get; set; }
        public string? Fields { get; set; } // Simple string now, no Dictionary
    }
}