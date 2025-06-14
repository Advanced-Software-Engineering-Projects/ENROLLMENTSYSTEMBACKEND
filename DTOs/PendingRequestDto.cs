namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class PendingRequestDto
    {
        public required string RequestId { get; set; }
        public required string StudentId { get; set; }
        public required string CourseId { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
