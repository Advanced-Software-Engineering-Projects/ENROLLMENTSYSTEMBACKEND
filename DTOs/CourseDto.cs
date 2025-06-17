using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class CourseDto
    {
        public string CourseId { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Program { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<CoursePrerequisiteDto> Prerequisites { get; set; } = new List<CoursePrerequisiteDto>();
    }

    public class CoursePrerequisiteDto
    {
        public int Id { get; set; }
        public string CourseId { get; set; } = string.Empty;
        public string PrerequisiteCourseId { get; set; } = string.Empty;
        public CourseDto PrerequisiteCourse { get; set; } = null!;
    }
}