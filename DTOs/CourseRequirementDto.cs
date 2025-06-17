namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class CourseRequirementDto
    {
        public string CourseId { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public bool IsSatisfied { get; set; }
    }
}
