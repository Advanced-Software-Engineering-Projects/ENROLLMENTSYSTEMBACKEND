using System;
using System.Collections.Generic;

namespace ExternalFormsService.DTOs
{
    public class ExternalFormSubmissionDto
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string FormType { get; set; }
        public Dictionary<string, string> FormData { get; set; }
        public List<string> Attachments { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string EmailStatus { get; set; }
    }
} 