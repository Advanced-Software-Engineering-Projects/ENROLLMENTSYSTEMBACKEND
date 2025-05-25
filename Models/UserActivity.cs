using System.ComponentModel.DataAnnotations;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class UserActivity
    {
        [Key]
        public int ActivityId { get; set; }
        public string UserId { get; set; }
        public string UserType { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
    }
}