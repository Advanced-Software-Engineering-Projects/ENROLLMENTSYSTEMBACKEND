namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class GraduationApplicationDto
    {
        public string ApplicationId { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}