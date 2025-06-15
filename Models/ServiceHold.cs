namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class ServiceHold
    {
        public string HoldId { get; set; } = Guid.NewGuid().ToString();
        public string StudentId { get; set; }
        public int ServiceId { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public Student Student { get; set; }
        public Service Service { get; set; }
    }
}