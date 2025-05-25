namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class AcademicRecordsDto
    {
        public string StudentId { get; set; }
        public string StudentEmail { get; set; }
        public double Gpa { get; set; }
        public List<EnrollmentDto> Enrollments { get; set; }
    }

    public class EnrollmentDto
    {
        public string EnrollmentId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Semester { get; set; }
        public string Grade { get; set; }
        public int Year { get; set; }
    }
}