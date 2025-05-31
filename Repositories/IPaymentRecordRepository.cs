using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IPaymentRecordRepository
    {
        Task<List<PaymentRecord>> GetPaymentRecordsByStudentIdAsync(string studentId);
        Task AddPaymentRecordAsync(PaymentRecord paymentRecord);
    }
}
