using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly FinancialAndAdminDbContext _context;

        public AdminRepository(FinancialAndAdminDbContext context) => _context = context;

        public async Task<Admin> GetAdminByUsernameAsync(string username) => await _context.Admins.FirstOrDefaultAsync(a => a.Username == username);
        public async Task<Admin> GetAdminByIdAsync(string adminId) => await _context.Admins.FindAsync(adminId);
        public async Task AddAdminAsync(Admin admin) { await _context.Admins.AddAsync(admin); await _context.SaveChangesAsync(); }
    }
}