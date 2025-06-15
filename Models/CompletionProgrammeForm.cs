using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class CompletionProgrammeForm : FormSubmission
    {
        public string Comments { get; set; }
        public bool IsCompleted { get; set; }
        public string ProgramCode { get; set; }
        
        [ForeignKey("Program")]
        public string ProgramId { get; set; }
        public Programs Program { get; set; }
    }
}