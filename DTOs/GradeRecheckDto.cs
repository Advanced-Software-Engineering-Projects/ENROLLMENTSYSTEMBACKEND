namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class GradeRecheckDto
    {
        public string StudentId { get; set; }
        public string CourseCode { get; set; }
        public string Semester { get; set; }
        public string Year { get; set; }
        public string Reason { get; set; }
        public string CurrentGrade { get; set; }
        public string EmailStatus { get; set; }
    }
}
