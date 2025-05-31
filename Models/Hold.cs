namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Hold
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string Service { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
