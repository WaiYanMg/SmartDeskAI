namespace Smart_Desk_AI.Models
{
    public class PriorityRule
    {
        public int Id { get; set; }
        public int? BusinessId { get; set; }
        public string RequestType { get; set; } = string.Empty;
        public string? ConditionField { get; set; }
        public string? ConditionOperator { get; set; }
        public string? ConditionValue { get; set; }
        public int Priority { get; set; } = 3;
        public bool AlwaysHuman { get; set; } = false;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Business? Business { get; set; }
    }
}