namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class AcademicRecordsDto
    {
        public string StudentId { get; set; }
        public string Email { get; set; }
        public double GPA { get; set; }
        public List<EnrollmentDto> Enrollments { get; set; }
    }
}
