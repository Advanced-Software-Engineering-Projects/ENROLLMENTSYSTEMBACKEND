namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Programs
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> RequiredCourseIds { get; set; } = new List<string>();
    }
}
