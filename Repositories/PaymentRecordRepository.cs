using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class PaymentRecordRepository : IPaymentRecordRepository
    {
        private readonly List<PaymentRecord> _paymentRecords = new List<PaymentRecord>();

        public async Task<List<PaymentRecord>> GetPaymentRecordsByStudentIdAsync(string studentId)
        {
            return await Task.FromResult(_paymentRecords.Where(pr => pr.StudentId == studentId).ToList());
        }

        public async Task AddPaymentRecordAsync(PaymentRecord paymentRecord)
        {
            _paymentRecords.Add(paymentRecord);
            await Task.CompletedTask;
        }
    }
}