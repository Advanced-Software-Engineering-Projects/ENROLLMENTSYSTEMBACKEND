using System;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class GradeRecheckRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public string CurrentGrade { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public Student Student { get; set; }
        public Course Course { get; set; }
        public Grade Grade { get; set; }
    }
}