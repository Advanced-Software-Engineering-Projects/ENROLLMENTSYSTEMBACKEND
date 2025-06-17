using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class AutoFillService : IAutoFillService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IProgramRepository _programRepository;
        private readonly ILogger<AutoFillService> _logger;

        public AutoFillService(
            IStudentRepository studentRepository,
            IEnrollmentRepository enrollmentRepository,
            IProgramRepository programRepository,
            ILogger<AutoFillService> logger)
        {
            _studentRepository = studentRepository;
            _enrollmentRepository = enrollmentRepository;
            _programRepository = programRepository;
            _logger = logger;
        }

        public async Task<FormAutoFillDataDto> GetAutoFillDataAsync(string studentId, string formType)
        {
            try
            {
                _logger.LogInformation("Getting auto-fill data for student {StudentId} and form type {FormType}", studentId, formType);

                var student = await _studentRepository.GetByIdAsync(studentId);
                if (student == null)
                {
                    throw new KeyNotFoundException($"Student not found with ID: {studentId}");
                }

                var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
                var program = await _programRepository.GetByIdAsync(student.ProgramId);

                var autoFillData = new FormAutoFillDataDto
                {
                    StudentId = studentId,
                    FormType = formType,
                    Fields = new Dictionary<string, string>(),
                    AdditionalData = new Dictionary<string, object>(),
                    LastUpdated = DateTime.UtcNow,
                    DataSource = "StudentRecordService"
                };

                // Add basic student information
                autoFillData.Fields.Add("StudentId", student.StudentId);
                autoFillData.Fields.Add("FirstName", student.FirstName);
                autoFillData.Fields.Add("LastName", student.LastName);
                autoFillData.Fields.Add("Email", student.Email);
                autoFillData.Fields.Add("Program", program?.Name ?? "N/A");
                autoFillData.Fields.Add("Year", student.Year.ToString());

                // Add form-specific data
                switch (formType.ToLower())
                {
                    case "grade-recheck":
                        await AddGradeRecheckDataAsync(autoFillData, enrollments);
                        break;
                    case "graduation":
                        await AddGraduationDataAsync(autoFillData, student, program);
                        break;
                    case "course-registration":
                        await AddCourseRegistrationDataAsync(autoFillData, student, program);
                        break;
                    default:
                        _logger.LogWarning("Unknown form type: {FormType}", formType);
                        break;
                }

                return autoFillData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting auto-fill data for student {StudentId}", studentId);
                throw;
            }
        }

        public async Task<bool> ValidateAutoFillDataAsync(string studentId, FormAutoFillDataDto data)
        {
            try
            {
                _logger.LogInformation("Validating auto-fill data for student {StudentId}", studentId);

                var student = await _studentRepository.GetByIdAsync(studentId);
                if (student == null)
                {
                    return false;
                }

                // Validate required fields
                var requiredFields = await GetRequiredFieldsAsync(data.FormType);
                foreach (var field in requiredFields)
                {
                    if (!data.Fields.ContainsKey(field) || string.IsNullOrEmpty(data.Fields[field]))
                    {
                        _logger.LogWarning("Missing required field {Field} for student {StudentId}", field, studentId);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating auto-fill data for student {StudentId}", studentId);
                return false;
            }
        }

        public async Task<Dictionary<string, string>> GetFormFieldMappingsAsync(string formType)
        {
            var mappings = new Dictionary<string, string>();

            switch (formType.ToLower())
            {
                case "grade-recheck":
                    mappings = new Dictionary<string, string>
                    {
                        { "StudentId", "Student ID" },
                        { "CourseCode", "Course Code" },
                        { "CurrentGrade", "Current Grade" },
                        { "Reason", "Reason for Recheck" }
                    };
                    break;
                case "graduation":
                    mappings = new Dictionary<string, string>
                    {
                        { "StudentId", "Student ID" },
                        { "Program", "Program" },
                        { "ExpectedGraduationDate", "Expected Graduation Date" },
                        { "ThesisTitle", "Thesis Title" }
                    };
                    break;
                case "course-registration":
                    mappings = new Dictionary<string, string>
                    {
                        { "StudentId", "Student ID" },
                        { "Program", "Program" },
                        { "Year", "Year" },
                        { "Semester", "Semester" }
                    };
                    break;
            }

            return await Task.FromResult(mappings);
        }

        private async Task AddGradeRecheckDataAsync(FormAutoFillDataDto autoFillData, IEnumerable<Enrollment> enrollments)
        {
            var currentEnrollments = enrollments.Where(e => e.Status == "Active");
            foreach (var enrollment in currentEnrollments)
            {
                autoFillData.AdditionalData.Add($"Course_{enrollment.CourseId}", new
                {
                    CourseCode = enrollment.Course?.CourseCode,
                    CourseName = enrollment.Course?.CourseName,
                    CurrentGrade = enrollment.Grade
                });
            }
        }

        private async Task AddGraduationDataAsync(FormAutoFillDataDto autoFillData, Student student, Program program)
        {
            autoFillData.Fields.Add("ExpectedGraduationDate", DateTime.Now.AddMonths(6).ToString("yyyy-MM-dd"));
            autoFillData.Fields.Add("ProgramCompletionStatus", "In Progress");
            autoFillData.AdditionalData.Add("ProgramDetails", new
            {
                ProgramName = program?.Name,
                TotalCredits = program?.TotalCredits,
                RequiredCredits = program?.RequiredCredits
            });
        }

        private async Task AddCourseRegistrationDataAsync(FormAutoFillDataDto autoFillData, Student student, Program program)
        {
            autoFillData.Fields.Add("Semester", GetCurrentSemester());
            autoFillData.AdditionalData.Add("ProgramRequirements", new
            {
                ProgramName = program?.Name,
                YearLevel = student.Year,
                RequiredCourses = program?.RequiredCourses
            });
        }

        private async Task<List<string>> GetRequiredFieldsAsync(string formType)
        {
            var requiredFields = new List<string> { "StudentId" };

            switch (formType.ToLower())
            {
                case "grade-recheck":
                    requiredFields.AddRange(new[] { "CourseCode", "CurrentGrade", "Reason" });
                    break;
                case "graduation":
                    requiredFields.AddRange(new[] { "Program", "ExpectedGraduationDate" });
                    break;
                case "course-registration":
                    requiredFields.AddRange(new[] { "Program", "Year", "Semester" });
                    break;
            }

            return requiredFields;
        }

        private string GetCurrentSemester()
        {
            var month = DateTime.Now.Month;
            return month >= 2 && month <= 6 ? "Semester 1" : "Semester 2";
        }
    }
} 