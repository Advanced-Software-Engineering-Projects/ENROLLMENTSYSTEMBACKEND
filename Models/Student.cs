using System.ComponentModel.DataAnnotations;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Student
    {
        [Key]
        [StringLength(9)]
        public string StudentId { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; } // Hashed

        public int ProgramVersionId { get; set; }

        public int EnrollmentYear { get; set; }
    }
}
