namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class DashboardMetricsDto
    {
        public int TotalStudents { get; set; }
        public int TotalEnrollments { get; set; }
        public int CompletedEnrollments { get; set; }
        public int PendingRequests { get; set; }
        public double CompletionRate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
