namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class ProgramAuditDto
    {
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public List<CourseStatusDto> CourseStatuses { get; set; } = new List<CourseStatusDto>();
        public double CompletionProgress { get; set; } 
        public bool IsEligible { get; set; }
        
    }
}