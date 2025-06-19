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
        private readonly ICourseRepository _courseRepository;

        public GradeService(IStudentRepository studentRepository, IEnrollmentRepository enrollmentRepository, ICourseRepository courseRepository)
        {
            _studentRepository = studentRepository;
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
        }

        public async Task<AcademicRecordsDto> GetAcademicRecordsAsync(string studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found.");
            }

            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var allCourses = await _courseRepository.GetAllCoursesAsync();

            var enrollmentDtos = enrollments.Select(e =>
            {
                var course = allCourses.FirstOrDefault(c => c.CourseId == e.CourseId);
                return new EnrollmentDto
                {
                    CourseId = e.CourseId,
                    CourseCode = course?.CourseCode,
                    CourseName = course?.CourseName,
                    StudentId = e.StudentId,
                    Grade = e.Grade,
                    Semester = e.Semester
                };
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
            var allCourses = await _courseRepository.GetAllCoursesAsync();

            return new TranscriptDto
            {
                StudentId = studentId,
                Enrollments = enrollments.Select(e =>
                {
                    var course = allCourses.FirstOrDefault(c => c.CourseId == e.CourseId);
                    return new EnrollmentDto
                    {
                        CourseId = e.CourseId,
                        CourseCode = course?.CourseCode,
                        CourseName = course?.CourseName,
                        StudentId = e.StudentId,
                        Grade = e.Grade,
                        Semester = e.Semester
                    };
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
                StudentName = "Student Name", // Adding required property
                ProgramName = "Program Name", // Adding required property
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
            if (string.IsNullOrEmpty(studentId))
            {
                throw new ArgumentException("Student ID cannot be null or empty", nameof(studentId));
            }

            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found");
            }

            var isEligible = await CheckGraduationEligibilityAsync(studentId);
            if (!isEligible)
            {
                throw new InvalidOperationException("Student is not eligible for graduation");
            }

            // In a real system, this would interact with a database or service
            return new GraduationApplicationDto
            {
                ApplicationId = Guid.NewGuid().ToString(),
                StudentId = studentId,
                StudentName = $"{student.FirstName} {student.LastName}",
                Program = student.Program,
                ApplicationDate = DateTime.UtcNow,
                Status = "Applied",
                ExpectedGraduationDate = DateTime.UtcNow.AddMonths(3)
            };
        }

        public async Task<GraduationApplicationDto> GetGraduationStatusAsync(string studentId)
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

            // In a real system, this would fetch from a database
            return new GraduationApplicationDto
            {
                ApplicationId = Guid.NewGuid().ToString(),
                StudentId = studentId,
                StudentName = $"{student.FirstName} {student.LastName}",
                Program = student.Program,
                ApplicationDate = DateTime.UtcNow.AddDays(-30), // Example: Applied 30 days ago
                Status = "Pending",
                ExpectedGraduationDate = DateTime.UtcNow.AddMonths(3)
            };
        }

        public async Task<List<Grade>> GetGradesByStudentIdAsync(string studentId)
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
                return new List<Grade>();
            }

            var courses = await _courseRepository.GetAllCoursesAsync();
            
            return enrollments
                .Where(e => !string.IsNullOrEmpty(e.Grade))
                .Select(e =>
                {
                    return new Grade
                    {
                        StudentId = e.StudentId,
                        CourseId = e.CourseId,
                        GradeValue = e.Grade
                    };
                })
                .ToList();
        }

        public async Task<FormSubmissionDto> UpdateGradeAsync(UpdateGradeDto updateGradeDto)
        {
            if (string.IsNullOrEmpty(updateGradeDto.SubmissionId))
            {
                throw new ArgumentException("SubmissionId cannot be null or empty");
            }

            // Remove any int parsing or conversion, treat SubmissionId as string GUID
            int submissionId;
            if (!int.TryParse(updateGradeDto.SubmissionId, out submissionId))
            {
                throw new ArgumentException("SubmissionId must be a valid integer");
            }

            // TODO: Implement actual update logic here to update grade in repository

            // For now, simulate update and return a FormSubmissionDto with SubmissionId as int
            return await Task.FromResult(new FormSubmissionDto
            {
                SubmissionId = submissionId,
                StudentId = "S123",
                FullName = "Student Name",
                Email = "student@example.com",
                Telephone = "123-456-7890",
                PostalAddress = "123 Main St",
                FormType = "Grade Update",
                Status = "Updated",
                EmailStatus = "Sent"
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
            if (enrollments == null || !enrollments.Any())
            {
                return 0.0;
            }

            var validEnrollments = enrollments
                .Where(e => !string.IsNullOrEmpty(e.Grade))
                .ToList();

            if (!validEnrollments.Any())
            {
                return 0.0;
            }

            var totalPoints = 0.0;
            var totalCredits = 0;

            foreach (var enrollment in validEnrollments)
            {
                var gradePoint = GetGradePoint(enrollment.Grade);
                var credits = enrollment.Course?.Credits ?? 3; // Access credits through Course navigation property
                totalPoints += gradePoint * credits;
                totalCredits += credits;
            }

            return totalCredits > 0 ? Math.Round(totalPoints / totalCredits, 2) : 0.0;
        }

        private double GetGradePoint(string grade)
        {
            if (string.IsNullOrEmpty(grade))
                return 0.0;

            return grade.ToUpper() switch
            {
                "A+" => 4.0,
                "A" => 4.0,
                "A-" => 3.7,
                "B+" => 3.3,
                "B" => 3.0,
                "B-" => 2.7,
                "C+" => 2.3,
                "C" => 2.0,
                "C-" => 1.7,
                "D+" => 1.3,
                "D" => 1.0,
                "F" => 0.0,
                _ => 0.0
            };
        }
    }
}