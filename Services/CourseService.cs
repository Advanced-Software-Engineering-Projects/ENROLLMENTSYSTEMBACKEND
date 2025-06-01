using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public CourseService(ICourseRepository courseRepository, IEnrollmentRepository enrollmentRepository)
        {
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<List<Course>> GetAvailableCoursesAsync(string studentId)
        {
            var allCourses = await _courseRepository.GetAllCoursesAsync();
            if (allCourses == null || allCourses.Count == 0)
            {
                // Optionally log or throw for debugging
                // throw new Exception("No courses found in the system.");
                return new List<Course>(); // This is what you currently return
            }

            // TODO: Filter out courses the student is already registered for, or not eligible for
            return allCourses;
        }

        public async Task<CourseDto> GetCourseDetailsAsync(string courseCode)
        {
            var course = await _courseRepository.GetCourseByCodeAsync(courseCode);
            if (course == null) return null;
            return new CourseDto
            {
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                Description = course.Description,
                Prerequisites = course.Prerequisites
            };
        }

        public async Task<List<Course>> GetRegisteredCoursesAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            if (enrollments == null || enrollments.Count == 0)
            {
                return new List<Course>();
            }

            var courses = new List<Course>();
            foreach (var enrollment in enrollments)
            {
                var course = await _courseRepository.GetCourseByIdAsync(enrollment.CourseId);
                if (course != null)
                {
                    courses.Add(course);
                }
            }
            return courses;
        }

        public async Task<bool> RegisterCourseAsync(string studentId, string courseCode)
        {
            // Placeholder: Check if course exists
            var course = await _courseRepository.GetCourseByCodeAsync(courseCode);
            return course != null;
        }

        public async Task<bool> RegisterCourseAsync(CourseRegistrationDto registrationDto)
        {
            // Assuming CourseRegistrationDto has StudentId and CourseCode properties
            var course = await _courseRepository.GetCourseByCodeAsync(registrationDto.CourseCode);
            if (course == null) return false;

            // Placeholder: In a real app, add to student's enrollments
            return true;
        }

        public async Task<bool> DropCourseAsync(string studentId, string courseCode)
        {
            var course = await _courseRepository.GetCourseByCodeAsync(courseCode);
            return course != null;
        }

        public async Task<bool> UnregisterCourseAsync(CourseRegistrationDto registrationDto)
        {
            // Assuming CourseRegistrationDto has StudentId and CourseCode properties
            var course = await _courseRepository.GetCourseByCodeAsync(registrationDto.CourseCode);
            if (course == null) return false;

            // Placeholder: In a real app, remove from student's enrollments
            return true;
        }

        public async Task<List<string>> GetPrerequisitesAsync(string courseCode)
        {
            var course = await _courseRepository.GetCourseByCodeAsync(courseCode);
            return course?.Prerequisites ?? new List<string>();
        }

        public async Task<List<Course>> GetCoursePrerequisitesAsync(string courseId)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if (course == null || course.Prerequisites == null) return new List<Course>();

            // Fetch prerequisite courses based on their codes
            var prerequisiteCourses = new List<Course>();
            foreach (var prereqCode in course.Prerequisites)
            {
                var prereqCourse = await _courseRepository.GetCourseByCodeAsync(prereqCode);
                if (prereqCourse != null)
                {
                    prerequisiteCourses.Add(prereqCourse);
                }
            }
            return prerequisiteCourses;
        }

        public async Task<List<Enrollment>> GetCourseHistoryAsync(string studentId)
        {
            var history = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            return history ?? new List<Enrollment>();
        }

        public async Task<bool> AddCourseAsync(CourseDto courseDto)
        {
            var course = new Course
            {
                CourseCode = courseDto.CourseCode,
                CourseName = courseDto.CourseName, // Fixed: Use CourseName instead of Name
                Description = courseDto.Description,
                Prerequisites = courseDto.Prerequisites
            };
            await _courseRepository.AddCourseAsync(course);
            return true;
        }

        public async Task<bool> UpdateCourseAsync(string courseCode, CourseDto updatedCourse)
        {
            var course = await _courseRepository.GetCourseByCodeAsync(courseCode);
            if (course == null) return false;

            course.CourseName = updatedCourse.CourseName; // Fixed: Use CourseName
            course.Description = updatedCourse.Description;
            course.Prerequisites = updatedCourse.Prerequisites;
            await _courseRepository.UpdateCourseAsync(course);
            return true;
        }

        public async Task<bool> DeleteCourseAsync(string courseCode)
        {
            var course = await _courseRepository.GetCourseByCodeAsync(courseCode);
            if (course == null) return false;

            await _courseRepository.DeleteCourseAsync(courseCode);
            return true;
        }
    }
}
