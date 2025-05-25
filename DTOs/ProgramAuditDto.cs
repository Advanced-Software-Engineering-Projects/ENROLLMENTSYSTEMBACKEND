namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class ProgramAuditDto
    {
        public string ProgramName { get; set; }
        public List<YearAuditDto> Years { get; set; }
        public List<CompletionDataDto> CompletionData { get; set; }
    }

    public class YearAuditDto
    {
        public int Year { get; set; }
        public List<CourseStatusDto> Courses { get; set; }
    }

    public class CourseStatusDto
    {
        public string CourseCode { get; set; }
        public string Status { get; set; }
    }

    public class CompletionDataDto
    {
        public string Year { get; set; }
        public double Completion { get; set; }
    }
}