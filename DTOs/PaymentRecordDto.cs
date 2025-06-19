namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class PaymentRecordDto
    {
        public required string Semester { get; set; }
        public required List<PaymentDetail> Payments { get; set; }

        public class PaymentDetail
        {
            public required string FeeId { get; set; }
            public decimal AmountPaid { get; set; }
            public DateTime PaymentDate { get; set; }
        }
    }
}
