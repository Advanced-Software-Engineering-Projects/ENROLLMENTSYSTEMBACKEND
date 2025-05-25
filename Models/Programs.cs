

using System.ComponentModel.DataAnnotations;

namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Programs
    {
        public int ProgramId { get; set; }
        public string Name { get; set; }
        public ICollection<ProgramVersion> ProgramVersions { get; set; }
    }
}