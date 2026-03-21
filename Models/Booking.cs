namespace Smart_Desk_AI.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public int CustomerId { get; set; }
        public int? StaffId { get; set; }
        public int? ServiceId { get; set; }
        public DateOnly BookingDate { get; set; }
        public TimeOnly TimeSlot { get; set; }
        public string Status { get; set; } = "PENDING";
        public string? AiDecision { get; set; }
        public string? AiReason { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Business? Business { get; set; }
        public Customer? Customer { get; set; }
        public Staff? Staff { get; set; }
        public Service? Service { get; set; }
    }
}