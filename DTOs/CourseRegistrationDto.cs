namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class CourseRegistrationDto
    {
        public string StudentId { get; set; }
        public string CourseCode { get; set; }
        public string CourseId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<string> CourseCodes { get; set; }
    }
}
