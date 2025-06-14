namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class CurrentFeeDto
    {
        public string? FeeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
    }
}
