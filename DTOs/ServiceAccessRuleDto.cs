using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class ServiceAccessRuleDto
    {
        public string HoldType { get; set; }
        public Dictionary<string, bool> AllowedServices { get; set; }
        public DateTime LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}