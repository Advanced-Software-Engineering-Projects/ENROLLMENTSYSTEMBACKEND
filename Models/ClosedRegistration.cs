using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class ClosedRegistration
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [NotMapped]
        public List<string> CourseCodes { get; set; } = new List<string>();
        
        public DateTime ClosedAt { get; set; } = DateTime.UtcNow;
    }
}