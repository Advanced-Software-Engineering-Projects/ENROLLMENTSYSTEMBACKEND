namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class ResendEmailDto
    {
        public string SubmissionId { get; set; } = string.Empty;
        public string FormType { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
    }
}