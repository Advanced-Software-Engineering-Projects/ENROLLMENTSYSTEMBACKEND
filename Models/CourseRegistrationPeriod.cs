using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class CourseRegistrationPeriod
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [NotMapped]
        public List<string> CourseCodes { get; set; } = new List<string>();
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}