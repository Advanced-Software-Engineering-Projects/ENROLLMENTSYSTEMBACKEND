using System;
using System.Collections.Generic;

namespace ExternalFormsService.DTOs
{
    public class GraduationApplicationDto
    {
        public string StudentId { get; set; }
        public string ApplicationType { get; set; } // "Graduation", "Compassionate", "Aegrotat", "Re-sit"
        public List<string> CourseIds { get; set; }
        public string Reason { get; set; }
        public Dictionary<string, string> SupportingDocuments { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
    }

    public class GraduationApplicationResponseDto
    {
        public string ApplicationId { get; set; }
        public string StudentId { get; set; }
        public string ApplicationType { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public bool IsQualified { get; set; }
        public DateTime ProcessedDate { get; set; }
    }
} 