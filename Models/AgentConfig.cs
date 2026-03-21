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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Business? Business { get; set; }
    }
}
