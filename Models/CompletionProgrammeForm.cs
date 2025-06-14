using System;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class CompletionProgrammeForm
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string ProgramCode { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Status { get; set; }
        public string? Comments { get; set; }
        public bool IsCompleted { get; set; }

        // Navigation properties
        public Student Student { get; set; }
        public Programs Program { get; set; }
    }
}