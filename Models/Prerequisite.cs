using System.ComponentModel.DataAnnotations;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Prerequisite
    {
        [Key]
        public int PrerequisiteId { get; set; }

        public int CourseId { get; set; }

        public int PrerequisiteCourseId { get; set; }
    }
}
