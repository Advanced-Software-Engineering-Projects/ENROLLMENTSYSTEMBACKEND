using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class TranscriptService : ITranscriptService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public TranscriptService(
            IStudentRepository studentRepository,
            ICourseRepository courseRepository,
            IEnrollmentRepository enrollmentRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<TranscriptDto> GetTranscriptAsync(string studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                throw new Exception($"Student with ID {studentId} not found.");
            }

            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var enrollmentDtos = new List<EnrollmentDto>();

            foreach (var enrollment in enrollments)
            {
                var course = await _courseRepository.GetCourseByIdAsync(enrollment.CourseId);
                if (course != null)
                {
                    enrollmentDtos.Add(new EnrollmentDto
                    {
                        CourseId = enrollment.CourseId,
                        CourseCode = enrollment.CourseCode,
                        StudentId = enrollment.StudentId,
                        CourseName = course.CourseName,
                        Semester = enrollment.Semester,
                        Grade = enrollment.Grade
                    });
                }
            }

            return new TranscriptDto
            {
                StudentId = studentId,
                Enrollments = enrollmentDtos
            };
        }

        public async Task<List<EnrollmentDto>> GetEnrollmentsByStudentIdAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var enrollmentDtos = new List<EnrollmentDto>();

            foreach (var enrollment in enrollments)
            {
                var course = await _courseRepository.GetCourseByIdAsync(enrollment.CourseId);
                if (course != null)
                {
                    enrollmentDtos.Add(new EnrollmentDto
                    {
                        CourseId = enrollment.CourseId,
                        CourseCode = enrollment.CourseCode,
                        StudentId = enrollment.StudentId,
                        CourseName = course.CourseName,
                        Semester = enrollment.Semester,
                        Grade = enrollment.Grade
                    });
                }
            }

            return enrollmentDtos;
        }

        public async Task<List<EnrollmentDto>> GetEnrollmentsBySemesterAsync(string studentId, string semester)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsBySemesterAsync(studentId, semester);
            var enrollmentDtos = new List<EnrollmentDto>();

            foreach (var enrollment in enrollments)
            {
                var course = await _courseRepository.GetCourseByIdAsync(enrollment.CourseId);
                if (course != null)
                {
                    enrollmentDtos.Add(new EnrollmentDto
                    {
                        CourseId = enrollment.CourseId,
                        CourseCode = enrollment.CourseCode,
                        StudentId = enrollment.StudentId,
                        CourseName = course.CourseName,
                        Semester = enrollment.Semester,
                        Grade = enrollment.Grade
                    });
                }
            }

            return enrollmentDtos;
        }

        public async Task<Dictionary<string, List<EnrollmentDto>>> GetEnrollmentsByProgramAsync(string programId)
        {
            var programCourses = await _courseRepository.GetCoursesByProgramAsync(programId);
            var result = new Dictionary<string, List<EnrollmentDto>>();

            foreach (var course in programCourses)
            {
                var enrollments = await _enrollmentRepository.GetEnrollmentsByCourseIdAsync(course.CourseId);
                var enrollmentDtos = enrollments.Select(e => new EnrollmentDto
                {
                    CourseId = e.CourseId,
                    CourseCode = e.CourseCode,
                    StudentId = e.StudentId,
                    CourseName = course.CourseName,
                    Semester = e.Semester,
                    Grade = e.Grade
                }).ToList();

                result[course.CourseName] = enrollmentDtos;
            }

            return result;
        }

        public async Task<byte[]> GenerateTranscriptPdfAsync(string studentId)
        {
            // This would normally generate a PDF, but for simplicity we'll just return a byte array
            var transcript = await GetTranscriptAsync(studentId);
            var pdfContent = System.Text.Encoding.UTF8.GetBytes($"Transcript for student {studentId}");
            return pdfContent;
        }

        public async Task<string> GetStudentGpaAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            if (!enrollments.Any())
                return "0.0";

            double totalPoints = 0;
            int totalCourses = 0;

            foreach (var enrollment in enrollments)
            {
                if (!string.IsNullOrEmpty(enrollment.Grade))
                {
                    if (int.TryParse(enrollment.Grade, out int grade))
                    {
                        // Simple GPA calculation (adjust as needed)
                        double points = grade / 20.0; // Convert to 0-5 scale
                        totalPoints += points;
                        totalCourses++;
                    }
                }
            }

            if (totalCourses == 0)
                return "0.0";

            double gpa = totalPoints / totalCourses;
            return gpa.ToString("F2");
        }

        public async Task<bool> ValidateTranscriptRequestAsync(string studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
                return false;

            // Check if student has any holds that would prevent transcript access
            // This is a simplified implementation
            return true;
        }
    }
}