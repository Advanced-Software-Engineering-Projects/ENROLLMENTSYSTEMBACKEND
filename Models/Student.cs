
namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Student
    {
        public string StudentId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int ProgramVersionId { get; set; }
        public int EnrollmentYear { get; set; }
        public ProgramVersion ProgramVersion { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Fee> Fees { get; set; }
    }
}