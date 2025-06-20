﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Programs
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        
        [NotMapped]
        public List<string> RequiredCourseIds { get; set; } = new List<string>();

        public int TotalCredits { get; set; }
        public int RequiredCredits { get; set; }
        
        [NotMapped]
        public List<string> RequiredCourses { get; set; } = new List<string>();
    }
}
