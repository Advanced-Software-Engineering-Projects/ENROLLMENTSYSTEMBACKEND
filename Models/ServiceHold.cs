namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class ServiceHold
    {
        public string HoldId { get; set; } = Guid.NewGuid().ToString();
        public string StudentId { get; set; }
        public string Service { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public Student Student { get; set; }
    }
}