namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Fee
    {
        public int FeeId { get; set; }
        public string StudentId { get; set; }
        public string Semester { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public Student Student { get; set; }
    }
}