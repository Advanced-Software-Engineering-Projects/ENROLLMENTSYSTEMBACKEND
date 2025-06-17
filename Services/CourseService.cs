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

        private async Task<CourseDto> MapToDto(Course course)
        {
            var prerequisites = await _prerequisiteRepository.GetPrerequisitesForCourseAsync(course.CourseCode);
            var prerequisiteDtos = new List<CoursePrerequisiteDto>();

            foreach (var prerequisite in prerequisites)
            {
                var prereqCourse = await _courseRepository.GetCourseByCodeAsync(prerequisite.PrerequisiteCourseCode);
                if (prereqCourse != null)
                {
                    prerequisiteDtos.Add(new CoursePrerequisiteDto
                    {
                        CourseId = course.CourseId,
                        PrerequisiteCourseId = prereqCourse.CourseId,
                        PrerequisiteCourse = new CourseDto
                        {
                            CourseId = prereqCourse.CourseId,
                            CourseCode = prereqCourse.CourseCode,
                            CourseName = prereqCourse.CourseName,
                            Name = prereqCourse.CourseName,
                            Program = prereqCourse.Program,
                            Year = prereqCourse.Year,
                            Description = prereqCourse.Description
                        }
                    });
                }
            }

            return new CourseDto
            {
                CourseId = course.CourseId,
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                Name = course.CourseName,
                Program = course.Program,
                Year = course.Year,
                Description = course.Description,
                Prerequisites = prerequisiteDtos
            };
        }

        public async Task<CourseDto?> GetCourseByIdAsync(string courseId)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                return null;
            }

            return await MapToDto(course);
        }

        public async Task<List<CourseDto>> GetCoursesByProgramAsync(string program)
        {
            var courses = await _courseRepository.GetCoursesByProgramAsync(program);
            var courseDtos = new List<CourseDto>();
            foreach (var course in courses)
            {
                courseDtos.Add(await MapToDto(course));
            }
            return courseDtos;
        }

        public async Task<List<CourseDto>> GetCoursesByProgramAndYearAsync(string program, int year)
        {
            var courses = await _courseRepository.GetCoursesByProgramAndYearAsync(program, year);
            var courseDtos = new List<CourseDto>();
            foreach (var course in courses)
            {
                courseDtos.Add(await MapToDto(course));
            }
            return courseDtos;
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
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found");
            }

            var allCourses = await _courseRepository.GetAllCoursesAsync();
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var completedCourses = enrollments.Where(e => e.Status == "Completed").Select(e => e.CourseCode).ToList();
            var enrolledCourses = enrollments.Where(e => e.Status == "Enrolled").Select(e => e.CourseCode).ToList();

            var availableCourses = new List<Course>();
            
            foreach (var course in allCourses.Where(c => c.Program == student.Program && 
                                                        !enrolledCourses.Contains(c.CourseCode) &&
                                                        !completedCourses.Contains(c.CourseCode)))
            {
                var prerequisites = await _prerequisiteRepository.GetPrerequisitesForCourseAsync(course.CourseCode);
                var hasAllPrerequisites = prerequisites.All(p => completedCourses.Contains(p.PrerequisiteCourseCode));
                
                if (hasAllPrerequisites)
                {
                    availableCourses.Add(course);
                }
            }

            var courseDtos = new List<CourseDto>();
            foreach (var course in availableCourses)
            {
                courseDtos.Add(await MapToDto(course));
            }
            return courseDtos;
        }

        public async Task<List<CourseDto>> GetRegisteredCoursesAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var enrolledCourseIds = enrollments
                .Where(e => e.Status == "Enrolled")
                .Select(e => e.CourseId)
                .ToList();

            var courses = await _courseRepository.GetAllCoursesAsync();
            var registeredCourses = courses
                .Where(c => enrolledCourseIds.Contains(c.CourseId))
                .ToList();

            var courseDtos = new List<CourseDto>();
            foreach (var course in registeredCourses)
            {
                courseDtos.Add(await MapToDto(course));
            }
            return courseDtos;
        }

        public async Task<List<CourseDto>> GetCourseHistoryAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var completedCourseIds = enrollments
                .Where(e => e.Status == "Completed")
                .Select(e => e.CourseId)
                .ToList();

            var courses = await _courseRepository.GetAllCoursesAsync();
            var completedCourses = courses
                .Where(c => completedCourseIds.Contains(c.CourseId))
                .ToList();

            var courseDtos = new List<CourseDto>();
            foreach (var course in completedCourses)
            {
                courseDtos.Add(await MapToDto(course));
            }
            return courseDtos;
        }

        public async Task<List<CourseRequirementDto>> GetCoursePrerequisitesAsync(string courseId)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                throw new InvalidOperationException("Course not found");
            }

            var prerequisites = await _prerequisiteRepository.GetPrerequisitesForCourseAsync(course.CourseCode);
            var prerequisiteCourses = new List<CourseRequirementDto>();

            foreach (var prerequisite in prerequisites)
            {
                var prereqCourse = await _courseRepository.GetCourseByCodeAsync(prerequisite.PrerequisiteCourseCode);
                if (prereqCourse != null)
                {
                    prerequisiteCourses.Add(new CourseRequirementDto
                    {
                        CourseId = prereqCourse.CourseId,
                        CourseCode = prereqCourse.CourseCode,
                        CourseName = prereqCourse.CourseName
                    });
                }
            }

            return prerequisiteCourses;
        }
    }
}