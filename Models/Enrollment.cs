using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public string Semester { get; set; }
        public string Status { get; set; }
        public string Grade { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}