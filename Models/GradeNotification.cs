using System;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class GradeNotification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public string OldGrade { get; set; }
        public string NewGrade { get; set; }
        public string NotificationType { get; set; } // New, Updated
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}