namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Enrollment
    {
        public string EnrollmentId { get; set; }
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public string CourseCode { get; set; } 
        public string Status { get; set; } // e.g., "Registered", "Dropped", "Pending", "Completed", "In Progress", "Not Started"
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Grade { get; set; }
        public string Semester { get; set; } // Added for enrollment data grouping
    }
}