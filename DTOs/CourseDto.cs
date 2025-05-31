namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class CourseDto
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Prerequisites { get; set; }
    }
}
