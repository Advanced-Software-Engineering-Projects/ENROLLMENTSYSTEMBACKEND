namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class ResitPaperRequestDto
    {
        public string StudentId { get; set; }
        public string CourseCode { get; set; }
        public string Semester { get; set; }
        public string Year { get; set; }
        public string Justification { get; set; }
        public string EmailStatus { get; set; }
    }
}
