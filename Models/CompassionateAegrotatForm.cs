using System;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class CompassionateAegrotatForm
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string CourseCode { get; set; }
        public string Reason { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Status { get; set; }
        public string? MedicalDocumentation { get; set; }
        public string? Comments { get; set; }

        // Navigation properties
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}