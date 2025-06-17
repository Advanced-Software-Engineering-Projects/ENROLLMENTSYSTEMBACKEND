using System;

namespace ExternalFormsService.DTOs
{
    public class FormStatusDto
    {
        public string FormId { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public DateTime LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
} 