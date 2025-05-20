using System.ComponentModel.DataAnnotations;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class ProgramCourses
    {
        [Key]
        public int ProgramCourseId { get; set; }

        public int ProgramVersionId { get; set; }

        public int CourseId { get; set; }

        public virtual ProgramVersion ProgramVersion { get; set; }
        public virtual Course Course { get; set; }
    }
}
