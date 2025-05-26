
namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public string SemesterOffered { get; set; }
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<ProgramCourse> ProgramCourses { get; set; }
        public ICollection<Prerequisite> Prerequisites { get; set; }
        public ICollection<Prerequisite> IsPrerequisiteFor { get; set; }
    }
}
