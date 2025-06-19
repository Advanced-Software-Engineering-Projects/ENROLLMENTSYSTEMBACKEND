using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class RegistrationPeriodRepository : IRegistrationPeriodRepository
    {
        private readonly EnrollmentInformationDbContext _context;

        public RegistrationPeriodRepository(EnrollmentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<RegistrationPeriod> GetCurrentRegistrationPeriodAsync()
        {
            return await _context.RegistrationPeriods
                .FirstOrDefaultAsync(rp => rp.IsActive);
        }

        public async Task<List<RegistrationPeriod>> GetAllRegistrationPeriodsAsync()
        {
            return await _context.RegistrationPeriods.ToListAsync();
        }

        public async Task<RegistrationPeriod> GetRegistrationPeriodByIdAsync(string periodId)
        {
            return await _context.RegistrationPeriods
                .FirstOrDefaultAsync(rp => rp.RegistrationPeriodId == periodId);
        }

        public async Task AddRegistrationPeriodAsync(RegistrationPeriod period)
        {
            _context.RegistrationPeriods.Add(period);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRegistrationPeriodAsync(RegistrationPeriod registrationPeriod)
        {
            _context.RegistrationPeriods.Update(registrationPeriod);
            await _context.SaveChangesAsync();
        }
    }
}