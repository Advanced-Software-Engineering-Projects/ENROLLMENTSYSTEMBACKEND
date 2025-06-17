using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ExternalFormsService.Models
{
    public class FormSubmission
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string StudentId { get; set; }
        
        [Required]
        public string FormType { get; set; }
        
        [Required]
        public Dictionary<string, string> FormData { get; set; }
        
        public List<string> Attachments { get; set; } = new();
        
        [Required]
        public string Status { get; set; } = "Pending";
        
        public string Comments { get; set; } = string.Empty;
        
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public string UpdatedBy { get; set; } = string.Empty;
        
        public string EmailStatus { get; set; } = "NotSent";
        
        public string SubmissionId { get; set; } = Guid.NewGuid().ToString();
    }
} 