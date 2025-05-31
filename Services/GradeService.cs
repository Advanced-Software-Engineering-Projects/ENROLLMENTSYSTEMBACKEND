using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class GradeService : IGradeService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public GradeService(IStudentRepository studentRepository, IEnrollmentRepository enrollmentRepository)
        {
            _studentRepository = studentRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<AcademicRecordsDto> GetAcademicRecordsAsync(string studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found.");
            }

            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var enrollmentDtos = enrollments.Select(e => new EnrollmentDto
            {
                CourseId = e.CourseId,
                Grade = e.Grade,
                Semester = e.Semester
            }).ToList();

            var gpa = CalculateGpa(enrollments);

            return new AcademicRecordsDto
            {
                StudentId = student.StudentId,
                Email = student.Email,
                GPA = gpa,
                Enrollments = enrollmentDtos
            };
        }

        public async Task<TranscriptDto> GetTranscriptAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            return new TranscriptDto
            {
                StudentId = studentId,
                Enrollments = enrollments.Select(e => new EnrollmentDto
                {
                    CourseId = e.CourseId,
                    Grade = e.Grade,
                    Semester = e.Semester
                }).ToList()
            };
        }

        public async Task<double> CalculateGPAAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            return CalculateGpa(enrollments);
        }

        public async Task<ProgramAuditDto> GetProgramAuditAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var completedCourses = enrollments
                .Where(e => e.Grade != "F" && !string.IsNullOrEmpty(e.Grade))
                .Select(e => e.CourseId)
                .ToList();
            var requiredCourses = new List<string> { "C1", "C2", "C3", "C4", "C5" }; // Placeholder required courses
            var isEligible = requiredCourses.All(rc => completedCourses.Contains(rc));

            var courseStatuses = requiredCourses.Select(rc => new CourseStatusDto
            {
                CourseId = rc,
                CourseName = $"Course {rc}", // Placeholder for course name
                Status = completedCourses.Contains(rc) ? "Completed" : "Pending",
                Grade = enrollments.FirstOrDefault(e => e.CourseId == rc)?.Grade
            }).ToList();

            return new ProgramAuditDto
            {
                StudentId = studentId,
                CourseStatuses = courseStatuses,
                CompletionProgress = (double)completedCourses.Count / requiredCourses.Count,
                IsEligible = isEligible
            };
        }

        public async Task<EnrollmentStatusDto> GetEnrollmentStatusAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var currentEnrollments = enrollments
                .Where(e => e.Semester == "CurrentSemester") // Example logic for current semester
                .Select(e => e.CourseId)
                .ToList();

            return new EnrollmentStatusDto
            {
                CurrentCourses = currentEnrollments
            };
        }

        public async Task<bool> CheckGraduationEligibilityAsync(string studentId)
        {
            var audit = await GetProgramAuditAsync(studentId);
            return audit.IsEligible;
        }

        public async Task<GraduationApplicationDto> ApplyForGraduationAsync(string studentId)
        {
            // Placeholder: In a real system, this would interact with a database or service
            return await Task.FromResult(new GraduationApplicationDto
            {
                StudentId = studentId,
                Status = "Applied"
            });
        }

        public async Task<GraduationApplicationDto> GetGraduationStatusAsync(string studentId)
        {
            // Placeholder: In a real system, this would fetch from a database
            return await Task.FromResult(new GraduationApplicationDto
            {
                StudentId = studentId,
                Status = "Pending"
            });
        }

        public async Task<List<Grade>> GetGradesByStudentIdAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            return enrollments.Select(e => new Grade
            {
                CourseId = e.CourseId,
                GradeValue = e.Grade
            }).ToList();
        }

        public async Task<FormSubmissionDto> UpdateGradeAsync(UpdateGradeDto updateGradeDto)
        {
            // Placeholder: In a real system, this would update the grade in the repository
            return await Task.FromResult(new FormSubmissionDto
            {
                SubmissionId = updateGradeDto.SubmissionId,
                Status = "Updated"
            });
        }

        public async Task<List<GpaTrendDto>> GetGpaTrendAsync(string studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var gpaTrend = enrollments
                .GroupBy(e => e.Semester)
                .Select(g => new GpaTrendDto
                {
                    Semester = g.Key,
                    GPA = CalculateGpa(g.ToList())
                })
                .OrderBy(g => g.Semester)
                .ToList();

            if (gpaTrend.Count == 0)
            {
                throw new InvalidOperationException("No GPA data available.");
            }

            return gpaTrend;
        }

        private double CalculateGpa(List<Enrollment> enrollments)
        {
            if (enrollments.Count == 0) return 0.0;

            var gradePoints = enrollments.Select(e => GetGradePoint(e.Grade)).ToList();
            return gradePoints.Average();
        }

        private double GetGradePoint(string grade)
        {
            return grade switch
            {
                "A" => 4.0,
                "B" => 3.0,
                "C" => 2.0,
                "D" => 1.0,
                "F" => 0.0,
                _ => 0.0
            };
        }
    }
}