namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class FormSubmissionDto
    {
        public string SubmissionId { get; set; }
        public string StudentId { get; set; }
        public string FormType { get; set; }
        public string Status { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}
