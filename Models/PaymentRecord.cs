namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class PaymentRecord
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string FeeId { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Semester { get; set; }
    }
}
