namespace Smart_Desk_AI.Models
{
    public class AgentConfig
    {
       public int Id { get; set; }
        public int BusinessId { get; set; }
        public string AgentName { get; set; } = string.Empty;
        public string Personality { get; set; } = string.Empty;
        public string Tone { get; set; } = string.Empty;
        public string? WelcomeMessage { get; set; }
        public string? CustomRules { get; set; }
        public string Language { get; set; } = "English";
        public string Task { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int AutoApproveThreshold { get; set; } = 85;
        public bool AlwaysHumanReview { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Business? Business { get; set; }
    }
}
