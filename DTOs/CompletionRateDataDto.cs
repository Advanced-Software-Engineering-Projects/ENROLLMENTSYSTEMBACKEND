namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class CompletionRateDataDto
    {
        public required string Semester { get; set; }
        public double CompletionRate { get; set; }
        public int TotalEnrollments { get; set; }
        public int CompletedEnrollments { get; set; }
        public int FailedEnrollments { get; set; }
        public int InProgressEnrollments { get; set; }
    }
}
