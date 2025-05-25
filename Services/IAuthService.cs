using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IAuthService
    {
        Task<UserDto> LoginAsync(LoginDto loginDto);
    }
}