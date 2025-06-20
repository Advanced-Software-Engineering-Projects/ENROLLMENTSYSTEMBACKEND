﻿using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IPrerequisiteRepository _prerequisiteRepository;

        public EnrollmentService(
            ICourseRepository courseRepository,
            IStudentRepository studentRepository,
            IEnrollmentRepository enrollmentRepository,
            IPrerequisiteRepository prerequisiteRepository)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _enrollmentRepository = enrollmentRepository;
            _prerequisiteRepository = prerequisiteRepository;
        }

        private CourseDto MapToDto(Course course)
        {
            return new CourseDto
            {
                CourseId = course.CourseId,
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                Name = course.CourseName,
                Program = course.Program,
                Year = course.Year,
                Description = course.Description,
                IsActive = course.IsActive,
                Prerequisites = course.Prerequisites.Select(p => new CoursePrerequisiteDto
                {
                    Id = p.Id,
                    CourseId = p.CourseId,
                    PrerequisiteCourseId = p.PrerequisiteCourseId,
                    PrerequisiteCourse = new CourseDto
                    {
                        CourseId = p.PrerequisiteCourse.CourseId,
                        CourseCode = p.PrerequisiteCourse.CourseCode,
                        CourseName = p.PrerequisiteCourse.CourseName,
                        Name = p.PrerequisiteCourse.CourseName,
                        Program = p.PrerequisiteCourse.Program,
                        Year = p.PrerequisiteCourse.Year,
                        Description = p.PrerequisiteCourse.Description
                    }
                }).ToList()
            };
        }

        public async Task<IEnumerable<CourseDto>> GetEnrolledCoursesAsync(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                throw new ArgumentException("Student ID cannot be null or empty", nameof(studentId));
            }

            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found");
            }

            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            if (!enrollments.Any())
            {
                return new List<CourseDto>();
            }

            var enrolledEnrollments = enrollments.Where(e => e.Status == "Completed").ToList();
            if (!enrolledEnrollments.Any())
            {
                return new List<CourseDto>();
            }

            var courseCodes = enrolledEnrollments.Select(e => e.CourseCode).ToList();
            var allCourses = await _courseRepository.GetAllCoursesAsync();
            var enrolledCourses = allCourses.Where(c => courseCodes.Contains(c.CourseCode));

            return enrolledCourses.Select(MapToDto).ToList();
        }

        public async Task<IEnumerable<CourseDto>> GetDroppedCoursesAsync(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                throw new ArgumentException("Student ID cannot be null or empty", nameof(studentId));
            }

            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found");
            }

            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            if (!enrollments.Any())
            {
                return new List<CourseDto>();
            }

            var droppedEnrollments = enrollments.Where(e => e.Status == "Dropped").ToList();
            if (!droppedEnrollments.Any())
            {
                return new List<CourseDto>();
            }

            var courseCodes = droppedEnrollments.Select(e => e.CourseCode).ToList();
            var allCourses = await _courseRepository.GetAllCoursesAsync();
            var droppedCourses = allCourses.Where(c => courseCodes.Contains(c.CourseCode));

            return droppedCourses.Select(MapToDto).ToList();
        }

        public async Task<IEnumerable<CourseDto>> GetAvailableCoursesAsync(string studentId, string program)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                throw new ArgumentException("Student ID cannot be null or empty", nameof(studentId));
            }

            if (string.IsNullOrEmpty(program))
            {
                throw new ArgumentException("Program cannot be null or empty", nameof(program));
            }

            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found");
            }

            if (student.Program != program)
            {
                throw new InvalidOperationException($"Student is not enrolled in program: {program}");
            }

            var allCourses = await _courseRepository.GetAllCoursesAsync();
            if (!allCourses.Any())
            {
                return new List<CourseDto>();
            }

            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var enrolledCourseCodes = enrollments
                .Where(e => e.Status == "Enrolled" || e.Status == "Completed")
                .Select(e => e.CourseCode)
                .ToList();

            //var availableCourses = allCourses
            //    .Where(c => c.Program == program && 
            //               //c.IsActive && 
            //               !enrolledCourseCodes.Contains(c.CourseCode));
            var availableCourses = allCourses
    .Where(c =>
    {
        bool programMatch = c.Program == program;
        bool notEnrolled = !enrolledCourseCodes.Contains(c.CourseCode);

        Console.WriteLine($"Checking Course: {c.CourseCode}");
        Console.WriteLine($"  Program: {c.Program} == {program} => {programMatch}");
        Console.WriteLine($"  Not Enrolled/Completed: {!enrolledCourseCodes.Contains(c.CourseCode)} => {notEnrolled}");

        return programMatch && notEnrolled;
    });


            return availableCourses.Select(MapToDto).ToList();
        }

        public async Task<IEnumerable<string>> GetPrerequisiteGraphAsync(string courseCode)
        {
            var course = await _courseRepository.GetCourseByCodeAsync(courseCode);
            if (course == null)
            {
                return Enumerable.Empty<string>();
            }

            return course.Prerequisites.Select(p => p.PrerequisiteCourse.CourseCode);
        }

        public async Task EnrollAsync(string studentId, string courseCode, string semester)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found.");
            }

            var course = await _courseRepository.GetCourseByCodeAsync(courseCode);
            if (course == null)
            {
                throw new InvalidOperationException("Course not found.");
            }

            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            if (enrollments.Any(e => e.CourseCode == courseCode && e.Status == "Enrolled"))
            {
                throw new InvalidOperationException("Already enrolled in this course.");
            }

            var completedCourses = enrollments.Where(e => e.Status == "Completed").Select(e => e.CourseCode).ToList();
            var unmetPrerequisites = course.Prerequisites
                .Where(p => !completedCourses.Contains(p.PrerequisiteCourse.CourseCode))
                .ToList();

            if (unmetPrerequisites.Any())
            {
                throw new InvalidOperationException("Prerequisites not met.");
            }

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseCode = courseCode,
                Semester = semester,
                Status = "Enrolled",
                EnrollmentDate = DateTime.UtcNow
            };

            await _enrollmentRepository.AddEnrollmentAsync(enrollment);
        }

        public async Task DropAsync(string studentId, string courseCode)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var enrollment = enrollments.FirstOrDefault(e => e.CourseCode == courseCode && e.Status == "Enrolled");
            if (enrollment == null)
            {
                throw new InvalidOperationException("Course not enrolled.");
            }

            enrollment.Status = "Dropped";
            await _enrollmentRepository.UpdateEnrollmentAsync(enrollment);
        }
    }
}