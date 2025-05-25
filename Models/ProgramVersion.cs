
namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class ProgramVersion
    {
        public int ProgramVersionId { get; set; }
        public int ProgramId { get; set; }
        public int HandbookYear { get; set; }
        public int TotalCreditsRequired { get; set; }
        public Programs Program { get; set; }
        public ICollection<ProgramCourse> ProgramCourses { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}