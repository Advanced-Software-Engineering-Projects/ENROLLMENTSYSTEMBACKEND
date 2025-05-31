using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Data;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EnrollmentInfromation _context;

        public UserRepository(EnrollmentInfromation context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}