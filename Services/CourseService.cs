using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class CourseService : ICourseService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IPrerequisiteRepository _prerequisiteRepository;

        public CourseService(IEnrollmentRepository enrollmentRepository, IPrerequisiteRepository prerequisiteRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _prerequisiteRepository = prerequisiteRepository;
        }

        public async Task<List<CourseDto>> GetAvailableCoursesAsync(string studentId, string semester)
        {
            // Get courses offered in the specified semester
            var allCourses = await _enrollmentRepository.GetCoursesBySemesterAsync(semester);
            // Get courses the student is already enrolled in
            var enrolledCourses = await _enrollmentRepository.GetEnrollmentsByStudentAsync(studentId);

            // Filter out courses the student is enrolled in
            var availableCourses = allCourses
                .Where(c => !enrolledCourses.Any(e => e.CourseId == c.CourseId && e.Semester == semester))
                .Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    Code = c.Code,
                    Name = c.Name,
                    Credits = c.Credits,
                    SemesterOffered = c.SemesterOffered
                })
                .ToList();

            return availableCourses;
        }

        public async Task EnrollCourseAsync(string studentId, int courseId, string semester)
        {
            // Basic validation: Check if already enrolled
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentAsync(studentId);
            if (enrollments.Any(e => e.CourseId == courseId && e.Semester == semester))
            {
                throw new InvalidOperationException("Student is already enrolled in this course for the semester.");
            }

            await _enrollmentRepository.AddEnrollmentAsync(studentId, courseId, semester);
        }

        public async Task DropCourseAsync(string studentId, int courseId, string semester)
        {
            // Check if the student is enrolled before attempting to drop
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentAsync(studentId);
            if (!enrollments.Any(e => e.CourseId == courseId && e.Semester == semester))
            {
                throw new InvalidOperationException("Student is not enrolled in this course for the semester.");
            }

            await _enrollmentRepository.RemoveEnrollmentAsync(studentId, courseId, semester);
        }

        public async Task<List<PrerequisiteDto>> GetPrerequisitesAsync(int courseId)
        {
            return await _prerequisiteRepository.GetPrerequisitesAsync(courseId);
        }

        public async Task<PrerequisiteGraphDto> GetPrerequisiteGraphAsync()
        {
            return await _prerequisiteRepository.GetPrerequisiteGraphAsync();
        }
    }
}