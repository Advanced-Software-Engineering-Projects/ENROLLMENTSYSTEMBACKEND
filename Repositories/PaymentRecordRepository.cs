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
        private readonly EnrollmentInfromation _context;

        public PaymentRecordRepository(EnrollmentInfromation context)
        {
            _context = context;
        }

        public async Task<List<PaymentRecord>> GetPaymentRecordsByStudentIdAsync(string studentId)
        {
            return await _context.PaymentRecords.Where(pr => pr.StudentId == studentId).ToListAsync();
        }

        public async Task AddPaymentRecordAsync(PaymentRecord paymentRecord)
        {
            _context.PaymentRecords.Add(paymentRecord);
            await _context.SaveChangesAsync();
        }
    }
}
