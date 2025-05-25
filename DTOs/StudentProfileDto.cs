using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class StudentProfileDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string StudentId { get; set; }
        public string Dob { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Citizenship { get; set; }
        public string Program { get; set; }
        public string StudentLevel { get; set; }
        public string StudentCampus { get; set; }
        public string ExamSite { get; set; }
        public string MajorType { get; set; }
        public string Major1 { get; set; }
        public string Major2 { get; set; }
        public string Avatar { get; set; }
    }
}