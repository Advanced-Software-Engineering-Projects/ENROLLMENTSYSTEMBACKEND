namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Course
    {
        public string CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }
        public string Description { get; set; }
        public string Program { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsActive { get; set; }
        public List<string> Prerequisites { get; set; } = new List<string>();
    }
}
