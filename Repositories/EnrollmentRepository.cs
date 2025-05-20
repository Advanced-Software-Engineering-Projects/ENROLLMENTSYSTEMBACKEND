using Microsoft.EntityFrameworkCore;
using ENROLLMENTSYSTEMBACKEND.Data;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly StudentInformationDbContext _context;

        public EnrollmentRepository(StudentInformationDbContext context) => _context = context;

        public async Task<Enrollment> GetEnrollmentByIdAsync(int enrollmentId) => await _context.Enrollments.FindAsync(enrollmentId);
        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAsync(string studentId) => await _context.Enrollments.Where(e => e.StudentId == studentId).ToListAsync();
        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAndSemesterAsync(string studentId, string semester) => await _context.Enrollments.Where(e => e.StudentId == studentId && e.Semester == semester).ToListAsync();
        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAndStatusAsync(string studentId, string status) => await _context.Enrollments.Where(e => e.StudentId == studentId && e.Status == status).ToListAsync();
        public async Task AddEnrollmentAsync(Enrollment enrollment) { await _context.Enrollments.AddAsync(enrollment); await _context.SaveChangesAsync(); }
        public async Task UpdateEnrollmentAsync(Enrollment enrollment) { _context.Enrollments.Update(enrollment); await _context.SaveChangesAsync(); }
        public async Task DeleteEnrollmentAsync(int enrollmentId)
        {
            var enrollment = await GetEnrollmentByIdAsync(enrollmentId);
            if (enrollment != null) { _context.Enrollments.Remove(enrollment); await _context.SaveChangesAsync(); }
        }
    }
}