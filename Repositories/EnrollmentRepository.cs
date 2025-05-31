using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly List<Enrollment> _enrollments = new List<Enrollment>();

        public async Task<List<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await Task.FromResult(_enrollments.ToList());
        }

        public async Task<List<Enrollment>> GetEnrollmentsAsync()
        {
            // Assuming GetEnrollmentsAsync is equivalent to GetAllEnrollmentsAsync
            return await Task.FromResult(_enrollments.ToList());
        }

        public async Task<List<Enrollment>> GetEnrollmentsByStudentIdAsync(string studentId)
        {
            return await Task.FromResult(_enrollments.Where(e => e.StudentId == studentId).ToList());
        }

        public async Task<List<Enrollment>> GetEnrollmentsByCourseIdAsync(string courseId)
        {
            return await Task.FromResult(_enrollments.Where(e => e.CourseId == courseId).ToList());
        }

        public async Task<Enrollment> GetEnrollmentByIdAsync(string enrollmentId)
        {
            return await Task.FromResult(_enrollments.FirstOrDefault(e => e.EnrollmentId == enrollmentId));
        }

        public async Task AddEnrollmentAsync(Enrollment enrollment)
        {
            enrollment.EnrollmentId = Guid.NewGuid().ToString();
            _enrollments.Add(enrollment);
            await Task.CompletedTask;
        }

        public async Task UpdateEnrollmentAsync(Enrollment enrollment)
        {
            var existing = _enrollments.FirstOrDefault(e => e.EnrollmentId == enrollment.EnrollmentId);
            if (existing != null)
            {
                existing.Status = enrollment.Status;
                // Update other properties if needed
            }
            await Task.CompletedTask;
        }

        public async Task DeleteEnrollmentAsync(string enrollmentId)
        {
            var enrollment = _enrollments.FirstOrDefault(e => e.EnrollmentId == enrollmentId);
            if (enrollment != null)
            {
                _enrollments.Remove(enrollment);
            }
            await Task.CompletedTask;
        }
    }
}