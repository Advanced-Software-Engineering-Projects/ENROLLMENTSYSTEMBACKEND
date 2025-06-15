using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Course
    {
        public string CourseId { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Program { get; set; } = string.Empty;
        public int Year { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsActive { get; set; }
        
        [NotMapped]
        public List<string> Prerequisites { get; set; } = new List<string>();
    }
}
