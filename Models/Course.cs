using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Course
    {
        [Key]
        public string CourseId { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Program { get; set; } = string.Empty;
        public int Year { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsActive { get; set; }
        
        // Navigation property for prerequisites
        public virtual ICollection<CoursePrerequisite> Prerequisites { get; set; } = new List<CoursePrerequisite>();
    }

    public class CoursePrerequisite
    {
        [Key]
        public int Id { get; set; }
        
        public string CourseId { get; set; } = string.Empty;
        public string PrerequisiteCourseId { get; set; } = string.Empty;
        
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } = null!;
        
        [ForeignKey("PrerequisiteCourseId")]
        public virtual Course PrerequisiteCourse { get; set; } = null!;
    }
}
