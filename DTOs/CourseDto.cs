namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class CourseDto
    {
        public string CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Name { get; set; }
        public string Program { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public List<string> Prerequisites { get; set; }
    }
}