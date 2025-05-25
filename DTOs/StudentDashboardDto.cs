
namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class StudentDashboardDto
    {
        public List<CourseDto> EnrolledCourses { get; set; }
        public int CompletedCoursesCount { get; set; }
        public double Gpa { get; set; }
        public List<GpaTrendDto> GpaTrend { get; set; }
    }

    public class GpaTrendDto
    {
        public string Semester { get; set; }
        public double Gpa { get; set; }
    }
}