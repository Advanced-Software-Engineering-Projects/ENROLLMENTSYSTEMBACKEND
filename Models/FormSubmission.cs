namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class FormSubmission
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string FormType { get; set; }
        public string Data { get; set; } // JSON-serialized form data
        public DateTime SubmissionDate { get; set; }
    }
}
