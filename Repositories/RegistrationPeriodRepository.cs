using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class RegistrationPeriodRepository : IRegistrationPeriodRepository
    {
        private readonly List<RegistrationPeriod> _periods = new List<RegistrationPeriod>(); // Simulated data

        public async Task<RegistrationPeriod> GetCurrentRegistrationPeriodAsync()
        {
            return await Task.FromResult(_periods.FirstOrDefault(p => p.IsActive));
        }

        public async Task<List<RegistrationPeriod>> GetAllRegistrationPeriodsAsync()
        {
            return await Task.FromResult(_periods);
        }

        public async Task<RegistrationPeriod> GetRegistrationPeriodByIdAsync(string periodId)
        {
            return await Task.FromResult(_periods.FirstOrDefault(p => p.RegistrationPeriodId == periodId));
        }

        public async Task AddRegistrationPeriodAsync(RegistrationPeriod period)
        {
            _periods.Add(period);
            await Task.CompletedTask;
        }

        public async Task UpdateRegistrationPeriodAsync(RegistrationPeriod period)
        {
            var existingPeriod = _periods.FirstOrDefault(p => p.RegistrationPeriodId == period.RegistrationPeriodId);
            if (existingPeriod != null)
            {
                existingPeriod.StartDate = period.StartDate;
                existingPeriod.EndDate = period.EndDate;
                existingPeriod.StartTime = period.StartTime;
                existingPeriod.EndTime = period.EndTime;
                existingPeriod.CourseCodes = period.CourseCodes;
                existingPeriod.IsActive = period.IsActive;
            }
            await Task.CompletedTask;
        }
    }
}