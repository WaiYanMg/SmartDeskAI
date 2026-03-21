namespace Smart_Desk_AI.Models
{
    public class LeaveBalance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public int TotalDays { get; set; } = 20;
        public int UsedDays { get; set; } = 0;
        public int RemainingDays { get; set; } = 20;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Employee? Employee { get; set; }
    }
}