using System.ComponentModel.DataAnnotations;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required]
        [StringLength(9)]
        public string StudentId { get; set; }

        public int CourseId { get; set; }

        [Required]
        [StringLength(20)]
        public string Semester { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } // Enrolled, Completed, Dropped

        [StringLength(2)]
        public string? Grade { get; set; } // Explicitly nullable
    }
}
