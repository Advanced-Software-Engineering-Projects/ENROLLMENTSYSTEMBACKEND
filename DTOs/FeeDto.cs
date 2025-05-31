namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class FeeDto
    {
        public string FeeId { get; set; }
        public string FeeType { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
    }
}
