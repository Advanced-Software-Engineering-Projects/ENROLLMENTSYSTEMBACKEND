namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Prerequisite
    {
        public int PrerequisiteId { get; set; }
        public int CourseId { get; set; }
        public int PrerequisiteCourseId { get; set; }
        public Course Course { get; set; }
        public Course PrerequisiteCourse { get; set; }
    }
}