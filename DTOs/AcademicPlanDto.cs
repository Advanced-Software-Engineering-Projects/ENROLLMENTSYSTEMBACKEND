namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class AcademicPlanDto
    {
        public List<SemesterPlanDto> Semesters { get; set; }
    }

    public class SemesterPlanDto
    {
        public int Semester { get; set; }
        public List<CourseDto> Courses { get; set; }
    }
}