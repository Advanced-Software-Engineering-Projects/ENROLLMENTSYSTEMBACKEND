namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class Fee
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public decimal Amount { get; set; }
        public string Semester { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
    }
}