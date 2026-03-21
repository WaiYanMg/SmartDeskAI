namespace Smart_Desk_AI.Models
{
    public class AgentDecision
    {
        public string AgentName { get; set; } = string.Empty;
        public string Decision { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string PolicyReference { get; set; } = string.Empty;
        public int ConfidenceScore { get; set; }
    }
}