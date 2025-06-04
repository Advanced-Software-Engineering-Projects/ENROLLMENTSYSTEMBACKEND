namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class FormApplicationDto
    {
        public string StudentId { get; set; }
        public string FormType { get; set; }
    }

    public class GradeRecheckApplicationDto
    {
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public string Reason { get; set; }
    }
}
