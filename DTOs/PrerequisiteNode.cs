namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class PrerequisiteNode
    {
        public CourseDto Course { get; set; }
        public bool IsMet { get; set; }
        public List<PrerequisiteNode> Prerequisites { get; set; } = new List<PrerequisiteNode>();
    }
}
