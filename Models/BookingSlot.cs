namespace Smart_Desk_AI.Models
{
    public class BookingSlot
    {
        public int Id { get; set; }
        public int? BusinessId { get; set; }
        public int? StaffId { get; set; }
        public DateOnly SlotDate { get; set; }
        public TimeOnly SlotTime { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int? BookingId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Business? Business { get; set; }
        public Staff? Staff { get; set; }
        public Booking? Booking { get; set; }
    }
}