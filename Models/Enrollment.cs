namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Enrollment
    {
        public string EnrollmentId { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // e.g., "Registered", "Dropped", "Pending", "Completed", "In Progress", "Not Started"
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty; // Added for enrollment data grouping
        public int Year { get; set; } // Added for enrollment year tracking

        // Navigation properties
        public virtual Course? Course { get; set; }
        public virtual Student? Student { get; set; }
    }
}