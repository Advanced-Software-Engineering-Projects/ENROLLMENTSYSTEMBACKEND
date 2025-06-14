using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IPrerequisiteRepository _prerequisiteRepository;

        public CourseService(
            ICourseRepository courseRepository,
            IEnrollmentRepository enrollmentRepository,
            IStudentRepository studentRepository,
            IPrerequisiteRepository prerequisiteRepository)
        {
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
            _studentRepository = studentRepository;
            _prerequisiteRepository = prerequisiteRepository;
        }

        public async Task<CourseDto?> GetCourseByIdAsync(string courseId)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                return null;
            }

            return new CourseDto
            {
                CourseId = course.CourseId,
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                Program = course.Program,
                Year = course.Year,
                Description = course.Description,
                Prerequisites = course.Prerequisites
            };
        }

        public async Task<List<CourseDto>> GetCoursesByProgramAsync(string program)
        {
            var courses = await _courseRepository.GetCoursesByProgramAsync(program);
            
            return courses.Select(c => new CourseDto
            {
                CourseId = c.CourseId,
                CourseCode = c.CourseCode,
                CourseName = c.CourseName,
                Program = c.Program,
                Year = c.Year,
                Description = c.Description,
                Prerequisites = c.Prerequisites
            }).ToList();
        }

        public async Task<List<CourseDto>> GetCoursesByProgramAndYearAsync(string program, int year)
        {
            var courses = await _courseRepository.GetCoursesByProgramAndYearAsync(program, year);
            
            return courses.Select(c => new CourseDto
            {
                CourseId = c.CourseId,
                CourseCode = c.CourseCode,
                CourseName = c.CourseName,
                Program = c.Program,
                Year = c.Year,
                Description = c.Description,
                Prerequisites = c.Prerequisites
            }).ToList();
        }

        public async Task<bool> RegisterCourseAsync(CourseRegistrationDto registrationDto)
        {
            // Implementation would check prerequisites, availability, etc.
            return await Task.FromResult(true);
        }

        public async Task<bool> UnregisterCourseAsync(CourseRegistrationDto registrationDto)
        {
            // Implementation would handle course withdrawal
            return await Task.FromResult(true);
        }

        public async Task<List<CourseDto>> GetAvailableCoursesAsync(string studentId)
        {
            // Implementation would filter courses based on student's program, completed prerequisites, etc.
            return await Task.FromResult(new List<CourseDto>());
        }

        public async Task<List<CourseDto>> GetRegisteredCoursesAsync(string studentId)
        {
            // Implementation would return courses the student is currently registered for
            return await Task.FromResult(new List<CourseDto>());
        }

        public async Task<List<CourseDto>> GetCourseHistoryAsync(string studentId)
        {
            // Implementation would return courses the student has completed
            return await Task.FromResult(new List<CourseDto>());
        }

        public async Task<List<CourseRequirementDto>> GetCoursePrerequisitesAsync(string courseId)
        {
            // Implementation would return prerequisites for a course
            return await Task.FromResult(new List<CourseRequirementDto>());
        }
    }
}