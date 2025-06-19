using ENROLLMENTSYSTEMBACKEND.Models;
using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Data;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EnrollmentInformationDbContext _context;

        public UserRepository(EnrollmentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}