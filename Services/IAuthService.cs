using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IAuthService
    {
        Task<User> AuthenticateAsync(string email, string password);
        Task<string> ResetPasswordAsync(string token, string newPassword);
    }
}
