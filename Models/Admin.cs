using System.ComponentModel.DataAnnotations;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Admin
    {
        [Key]
        [StringLength(10)]
        public string AdminId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; } // Hashed

        [Required]
        [StringLength(20)]
        public string Role { get; set; } // SAS_MANAGER, ADMIN

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
    }
}
