using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.IRepositories
{
    public interface IAdminRepository
    {
        Task<Admin> GetAdminByUsernameAsync(string username);
        Task<Admin> GetAdminByIdAsync(string adminId);
        Task AddAdminAsync(Admin admin);
    }
}
