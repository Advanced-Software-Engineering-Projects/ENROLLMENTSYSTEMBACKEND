namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class StudentDto
    {
        public string StudentId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int ProgramVersionId { get; set; }
        public int EnrollmentYear { get; set; }
    }
}
