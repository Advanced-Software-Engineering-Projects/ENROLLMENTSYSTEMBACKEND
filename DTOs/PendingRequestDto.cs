namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class PendingRequestDto
    {
        public string RequestId { get; set; }
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
