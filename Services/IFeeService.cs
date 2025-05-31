using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IFeeService
    {
        Task<List<CurrentFeeDto>> GetCurrentFeesByStudentIdAsync(string studentId);
        Task<List<PaymentRecordDto>> GetPaymentRecordsByStudentIdAsync(string studentId);
        Task<List<FeeHoldDto>> GetFeeHoldsByStudentIdAsync(string studentId);
        Task MarkFeeAsPaidAsync(string studentId, string feeId);
    }
}
