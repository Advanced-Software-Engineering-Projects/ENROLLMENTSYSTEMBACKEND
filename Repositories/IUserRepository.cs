using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}
