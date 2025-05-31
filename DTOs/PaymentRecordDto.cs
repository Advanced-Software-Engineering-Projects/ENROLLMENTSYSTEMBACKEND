namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class PaymentRecordDto
    {
        public string Semester { get; set; }
        public List<PaymentDetail> Payments { get; set; }

        public class PaymentDetail
        {
            public string FeeId { get; set; }
            public decimal AmountPaid { get; set; }
            public DateTime PaymentDate { get; set; }
        }
    }
}
