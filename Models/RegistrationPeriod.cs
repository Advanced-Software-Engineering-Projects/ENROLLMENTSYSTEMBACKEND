﻿﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class RegistrationPeriod
    {
        public string RegistrationPeriodId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        
        [NotMapped]
        public List<string> CourseCodes { get; set; } = new List<string>();
        
        public bool IsActive { get; set; }
    }
}
