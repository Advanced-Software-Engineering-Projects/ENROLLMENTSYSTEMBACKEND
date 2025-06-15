using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class CompassionateAegrotatForm : FormSubmission
    {
        public string MedicalDocumentation { get; set; }
        public string Comments { get; set; }
        
        [ForeignKey("Course")]
        public string CourseId { get; set; }
        public Course Course { get; set; }
    }
}