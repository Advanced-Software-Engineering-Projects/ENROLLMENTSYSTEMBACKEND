namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class ProgramAuditDto
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string ProgramName { get; set; }
        public List<CourseStatusDto> CourseStatuses { get; set; }
        public double CompletionProgress { get; set; } 
        public bool IsEligible { get; set; }
        
    }
}
