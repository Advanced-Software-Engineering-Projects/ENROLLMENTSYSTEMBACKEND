namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class PendingRequest
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string CourseCode { get; set; }
        public string RequestType { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}
