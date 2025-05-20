using System.ComponentModel.DataAnnotations;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class ProgramVersion
    {
        [Key]
        public int ProgramVersionId { get; set; }

        public int ProgramId { get; set; }

        public int HandbookYear { get; set; }

        public int TotalCreditsRequired { get; set; }

        public virtual Programs Program { get; set; }
    }
}
