namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class AdminDashboardDto
    {
        public int RegisteredStudents { get; set; }
        public int ActiveCourses { get; set; }
        public int PendingApprovals { get; set; }
        public List<PendingRequestDto> PendingRequests { get; set; }
        public List<EnrollmentDataDto> EnrollmentData { get; set; }
        public List<CompletionRateDto> CompletionRateData { get; set; }
    }

    public class PendingRequestDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string CourseCode { get; set; }
        public string RequestType { get; set; }
        public string Date { get; set; }
    }

    public class EnrollmentDataDto
    {
        public string Semester { get; set; }
        public int Students { get; set; }
    }

    public class CompletionRateDto
    {
        public string Semester { get; set; }
        public double Rate { get; set; }
    }
}