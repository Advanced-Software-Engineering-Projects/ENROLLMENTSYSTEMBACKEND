using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Fee
    {
        [Key]
        public int FeeId { get; set; }

        [Required]
        [StringLength(9)]
        public string StudentId { get; set; }

        [Required]
        [StringLength(20)]
        public string Semester { get; set; }

        [Precision(18, 2)] // Optional annotation for clarity
        public decimal Amount { get; set; }

        public bool IsPaid { get; set; }
    }
}
