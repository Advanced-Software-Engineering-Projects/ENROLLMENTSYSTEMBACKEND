using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Data;
using Microsoft.EntityFrameworkCore;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class CourseManagementRepository : ICourseManagementRepository
    {
        private readonly EnrollmentInformationDbContext _context;

        public CourseManagementRepository(EnrollmentInformationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseRegistrationPeriod>> GetOpenRegistrationsAsync()
        {
            return await _context.Set<CourseRegistrationPeriod>()
                .Where(r => r.IsActive)
                .ToListAsync();
        }

        public async Task<List<ClosedRegistration>> GetClosedRegistrationsAsync()
        {
            return await _context.Set<ClosedRegistration>()
                .ToListAsync();
        }

        public async Task AddOpenRegistrationAsync(CourseRegistrationPeriod registration)
        {
            _context.Set<CourseRegistrationPeriod>().Add(registration);
            await _context.SaveChangesAsync();
        }

        public async Task CloseRegistrationAsync(List<string> courseCodes)
        {
            // Retrieve all active registrations first
            var activeRegistrations = await _context.Set<CourseRegistrationPeriod>()
                .Where(r => r.IsActive)
                .ToListAsync();

            // Filter registrations in memory by course codes
            var registrationsToUpdate = activeRegistrations
                .Where(r => r.CourseCodes.Any(c => courseCodes.Contains(c)))
                .ToList();

            foreach (var registration in registrationsToUpdate)
            {
                // Get courses to close from this registration
                var coursesToClose = registration.CourseCodes
                    .Where(c => courseCodes.Contains(c))
                    .ToList();

                // Create closed registration record
                var closedRegistration = new ClosedRegistration
                {
                    CourseCodes = coursesToClose,
                    ClosedAt = DateTime.UtcNow
                };
                _context.Set<ClosedRegistration>().Add(closedRegistration);

                // Remove closed courses from open registration
                foreach (var course in coursesToClose)
                {
                    registration.CourseCodes.Remove(course);
                }

                // If no courses left in this registration period, mark it as inactive
                if (registration.CourseCodes.Count == 0)
                {
                    registration.IsActive = false;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetTotalRegistrationsCountAsync()
        {
            return await _context.Enrollments
                .Where(e => e.Status == "Registered")
                .CountAsync();
        }

        public async Task<int> GetTotalDroppedCoursesCountAsync()
        {
            return await _context.Enrollments
                .Where(e => e.Status == "Dropped")
                .CountAsync();
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses
                .Where(c => c.IsActive)
                .ToListAsync();
        }
    }
}