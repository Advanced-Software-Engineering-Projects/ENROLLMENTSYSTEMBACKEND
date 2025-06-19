namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class EnrollmentDataDto
    {
        public required string Semester { get; set; }
        public int EnrollmentCount { get; set; }
        public int ActiveEnrollments { get; set; }
        public int CompletedEnrollments { get; set; }
    }
}
