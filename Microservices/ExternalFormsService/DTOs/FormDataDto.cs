using System;
using System.Collections.Generic;

namespace ExternalFormsService.DTOs
{
    public class FormDataDto
    {
        public string FormType { get; set; }
        public Dictionary<string, string> Fields { get; set; }
        public List<string> Attachments { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
    }
} 