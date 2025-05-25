namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class ProgramCourse
    {
        public int ProgramCourseId { get; set; }
        public int ProgramVersionId { get; set; }
        public int CourseId { get; set; }
        public ProgramVersion ProgramVersion { get; set; }
        public Course Course { get; set; }
    }
}