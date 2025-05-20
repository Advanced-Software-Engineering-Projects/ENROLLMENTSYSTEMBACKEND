using System.ComponentModel.DataAnnotations;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class UserActivity
    {
        [Key]
        public int ActivityId { get; set; }

        [Required]
        [StringLength(10)]
        public string UserId { get; set; }

        [Required]
        [StringLength(20)]
        public string UserType { get; set; } // student, admin

        [Required]
        [StringLength(50)]
        public string Action { get; set; }

        public DateTime Timestamp { get; set; }

        public string Details { get; set; } // JSON string
    }
}
