using System.ComponentModel.DataAnnotations;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class FormSubmission
    {
        [Key]
        public string SubmissionId { get; set; }
        public string StudentId { get; set; }
        public string FormType { get; set; } // e.g., "reconsideration", "compassionateAegrotat"
        public string Status { get; set; } // e.g., "Pending", "Approved", "Rejected"
        public DateTime SubmissionDate { get; set; }
    }
}
