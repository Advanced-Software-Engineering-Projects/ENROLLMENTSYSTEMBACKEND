namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class CourseManagementDto
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public List<string> CourseCodes { get; set; } = new List<string>();
    }

    public class CloseCourseRegistrationDto
    {
        public List<string> CourseCodes { get; set; } = new List<string>();
    }

    public class OpenRegistrationDto
    {
        public List<CourseRegistrationPeriodDto> OpenRegistrations { get; set; } = new List<CourseRegistrationPeriodDto>();
    }

    public class ClosedRegistrationDto
    {
        public List<string> Courses { get; set; } = new List<string>();
        public string ClosedAt { get; set; }
    }

    public class CourseRegistrationPeriodDto
    {
        public List<string> Courses { get; set; } = new List<string>();
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
    }

    public class CourseRegistrationStatusDto
    {
        public List<CourseRegistrationPeriodDto> OpenRegistrations { get; set; } = new List<CourseRegistrationPeriodDto>();
        public List<ClosedRegistrationDto> ClosedRegistrations { get; set; } = new List<ClosedRegistrationDto>();
    }
}