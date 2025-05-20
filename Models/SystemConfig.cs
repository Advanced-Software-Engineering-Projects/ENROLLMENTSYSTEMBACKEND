using System.ComponentModel.DataAnnotations;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class SystemConfig
    {
        [Key]
        public int ConfigId { get; set; }

        [Required]
        [StringLength(50)]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
