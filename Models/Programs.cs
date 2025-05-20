using System.ComponentModel.DataAnnotations;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Programs
    {
        [Key]
        public int ProgramId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
