namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class TranscriptDto
    {
        public List<EnrollmentDto> Enrollments { get; set; }
        public string StudentId { get; set; }
        public double GPA { get; set; }
    }
}
