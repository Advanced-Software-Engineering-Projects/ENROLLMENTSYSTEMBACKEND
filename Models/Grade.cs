namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Grade
    {
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public string GradeValue { get; set; }

        // Navigation properties
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}