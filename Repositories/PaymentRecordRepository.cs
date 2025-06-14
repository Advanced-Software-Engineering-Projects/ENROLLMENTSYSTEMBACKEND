using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class PaymentRecordRepository : IPaymentRecordRepository
    {
        private readonly EnrollmentInformationDbContext _context;

        public PaymentRecordRepository(EnrollmentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PaymentRecord>> GetPaymentRecordsByStudentIdAsync(string studentId)
        {
            return await _context.PaymentRecords
                .Where(pr => pr.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<PaymentRecord> GetPaymentRecordByIdAsync(string paymentId)
        {
            return await _context.PaymentRecords
                .FirstOrDefaultAsync(pr => pr.Id == paymentId);
        }

        public async Task AddPaymentRecordAsync(PaymentRecord paymentRecord)
        {
            _context.PaymentRecords.Add(paymentRecord);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePaymentRecordAsync(PaymentRecord paymentRecord)
        {
            _context.PaymentRecords.Update(paymentRecord);
            await _context.SaveChangesAsync();
        }
    }
}