using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IRegistrationPeriodRepository
    {
        Task<RegistrationPeriod> GetCurrentRegistrationPeriodAsync();
        Task<List<RegistrationPeriod>> GetAllRegistrationPeriodsAsync();
        Task<RegistrationPeriod> GetRegistrationPeriodByIdAsync(string periodId);
        Task AddRegistrationPeriodAsync(RegistrationPeriod period);
        Task UpdateRegistrationPeriodAsync(RegistrationPeriod period);
    }
}
