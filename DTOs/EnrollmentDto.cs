namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class EnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public string Semester { get; set; }
        public string Status { get; set; }
        public string Grade { get; set; }
    }
}
