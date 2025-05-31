using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class FeeService : IFeeService
    {
        private readonly IFeeRepository _feeRepository;
        private readonly IPaymentRecordRepository _paymentRecordRepository;
        private readonly IFeeHoldRepository _feeHoldRepository;

        public FeeService(IFeeRepository feeRepository, IPaymentRecordRepository paymentRecordRepository, IFeeHoldRepository feeHoldRepository)
        {
            _feeRepository = feeRepository;
            _paymentRecordRepository = paymentRecordRepository;
            _feeHoldRepository = feeHoldRepository;
        }

        public async Task<List<CurrentFeeDto>> GetCurrentFeesByStudentIdAsync(string studentId)
        {
            var fees = await _feeRepository.GetFeesByStudentIdAsync(studentId);
            var currentSemester = "Spring 2024"; // Placeholder; replace with dynamic logic
            var currentFees = fees.Where(f => f.Semester == currentSemester).ToList();
            return currentFees.Select(f => new CurrentFeeDto
            {
                FeeId = f.Id,
                Amount = f.Amount,
                DueDate = f.DueDate,
                IsPaid = f.IsPaid
            }).ToList();
        }

        public async Task<List<PaymentRecordDto>> GetPaymentRecordsByStudentIdAsync(string studentId)
        {
            var paymentRecords = await _paymentRecordRepository.GetPaymentRecordsByStudentIdAsync(studentId);
            var groupedPayments = paymentRecords.GroupBy(pr => pr.Semester)
                .Select(g => new PaymentRecordDto
                {
                    Semester = g.Key,
                    Payments = g.Select(pr => new PaymentRecordDto.PaymentDetail
                    {
                        FeeId = pr.FeeId,
                        AmountPaid = pr.AmountPaid,
                        PaymentDate = pr.PaymentDate
                    }).ToList()
                }).ToList();
            return groupedPayments;
        }

        public async Task<List<FeeHoldDto>> GetFeeHoldsByStudentIdAsync(string studentId)
        {
            var feeHolds = await _feeHoldRepository.GetFeeHoldsByStudentIdAsync(studentId);
            return feeHolds.Select(fh => new FeeHoldDto
            {
                HoldId = fh.Id,
                Reason = fh.Reason,
                DateApplied = fh.DateApplied
            }).ToList();
        }

        public async Task MarkFeeAsPaidAsync(string studentId, string feeId)
        {
            var fee = await _feeRepository.GetFeeByIdAsync(feeId);
            if (fee == null || fee.StudentId != studentId)
            {
                throw new InvalidOperationException("Fee not found.");
            }
            if (fee.IsPaid)
            {
                throw new InvalidOperationException("Fee already paid.");
            }

            fee.IsPaid = true;
            await _feeRepository.UpdateFeeAsync(fee);

            var paymentRecord = new PaymentRecord
            {
                Id = Guid.NewGuid().ToString(),
                StudentId = studentId,
                FeeId = feeId,
                AmountPaid = fee.Amount,
                PaymentDate = DateTime.UtcNow,
                Semester = fee.Semester
            };
            await _paymentRecordRepository.AddPaymentRecordAsync(paymentRecord);
        }
    }
}