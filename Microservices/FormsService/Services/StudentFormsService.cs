using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using Microsoft.Extensions.Logging;

namespace ENROLLMENTSYSTEMBACKEND.Microservices.FormsService.Services
{
    public class StudentFormsService : IStudentFormsService
    {
        private readonly IFormRepository _formRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<StudentFormsService> _logger;

        public StudentFormsService(
            IFormRepository formRepository,
            IStudentRepository studentRepository,
            IGradeRepository gradeRepository,
            INotificationService notificationService,
            ILogger<StudentFormsService> logger)
        {
            _formRepository = formRepository;
            _studentRepository = studentRepository;
            _gradeRepository = gradeRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<FormSubmissionDto> SubmitReconsiderationFormAsync(ReconsiderationFormDto formDto)
        {
            _logger.LogInformation($"Processing reconsideration form for student {formDto.StudentId}");
            
            // Validate student exists
            var student = await _studentRepository.GetStudentByIdAsync(formDto.StudentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found");
            }

            // Validate course exists and student is enrolled
            var grade = await _gradeRepository.GetGradeAsync(formDto.StudentId, formDto.CourseCode);
            if (grade == null)
            {
                throw new InvalidOperationException("Student is not enrolled in the specified course");
            }

            // Create form submission
            var form = new FormSubmission
            {
                SubmissionId = Guid.NewGuid().ToString(),
                StudentId = formDto.StudentId,
                FullName = formDto.FullName,
                Email = formDto.Email,
                Telephone = formDto.Telephone,
                PostalAddress = formDto.PostalAddress,
                FormType = "Reconsideration",
                Status = "Submitted",
                EmailStatus = "Pending",
                SubmissionDate = DateTime.UtcNow
            };

            await _formRepository.AddFormAsync(form);
            
            // Send notification
            await _notificationService.NotifyFormApplicationStatusAsync(
                formDto.StudentId, 
                "Reconsideration", 
                "Submitted");

            return new FormSubmissionDto
            {
                Id = 0, // Will be set by database
                SubmissionId = int.Parse(form.SubmissionId),
                StudentId = form.StudentId,
                FullName = form.FullName,
                Email = form.Email,
                Telephone = form.Telephone,
                PostalAddress = form.PostalAddress,
                FormType = form.FormType,
                Status = form.Status,
                EmailStatus = form.EmailStatus,
                SubmissionDate = form.SubmissionDate
            };
        }

        public async Task<FormSubmissionDto> SubmitCompassionateAegrotatFormAsync(CompassionateAegrotatFormDto formDto)
        {
            _logger.LogInformation($"Processing compassionate/aegrotat form for student {formDto.StudentId}");
            
            // Validate student exists
            var student = await _studentRepository.GetStudentByIdAsync(formDto.StudentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found");
            }

            // Validate missed exams
            foreach (var exam in formDto.MissedExams)
            {
                if (!string.IsNullOrEmpty(exam.CourseCode))
                {
                    var grade = await _gradeRepository.GetGradeAsync(formDto.StudentId, exam.CourseCode);
                    if (grade == null)
                    {
                        throw new InvalidOperationException($"Student is not enrolled in course {exam.CourseCode}");
                    }
                }
            }

            // Create form submission
            var form = new FormSubmission
            {
                SubmissionId = Guid.NewGuid().ToString(),
                StudentId = formDto.StudentId,
                FullName = formDto.FullName,
                Email = formDto.Email,
                Telephone = formDto.Telephone,
                PostalAddress = formDto.PostalAddress,
                FormType = "CompassionateAegrotat",
                Status = "Submitted",
                EmailStatus = "Pending",
                SubmissionDate = DateTime.UtcNow
            };

            await _formRepository.AddFormAsync(form);
            
            // Send notification
            await _notificationService.NotifyFormApplicationStatusAsync(
                formDto.StudentId, 
                "CompassionateAegrotat", 
                "Submitted");

            return new FormSubmissionDto
            {
                Id = 0, // Will be set by database
                SubmissionId = int.Parse(form.SubmissionId),
                StudentId = form.StudentId,
                FullName = form.FullName,
                Email = form.Email,
                Telephone = form.Telephone,
                PostalAddress = form.PostalAddress,
                FormType = form.FormType,
                Status = form.Status,
                EmailStatus = form.EmailStatus,
                SubmissionDate = form.SubmissionDate
            };
        }

        public async Task<FormSubmissionDto> SubmitCompletionProgrammeFormAsync(CompletionProgrammeFormDto formDto)
        {
            _logger.LogInformation($"Processing completion programme form for student {formDto.StudentId}");
            
            // Validate student exists
            var student = await _studentRepository.GetStudentByIdAsync(formDto.StudentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found");
            }

            // Check if student is eligible for graduation
            bool isEligible = await CheckEligibilityAsync(formDto.StudentId, "CompletionProgramme");
            if (!isEligible)
            {
                throw new InvalidOperationException("Student is not eligible for graduation");
            }

            // Create form submission
            var form = new FormSubmission
            {
                SubmissionId = Guid.NewGuid().ToString(),
                StudentId = formDto.StudentId,
                FullName = formDto.FullName,
                Email = formDto.Email,
                Telephone = formDto.Telephone,
                PostalAddress = formDto.PostalAddress,
                FormType = "CompletionProgramme",
                Status = "Submitted",
                EmailStatus = "Pending",
                SubmissionDate = DateTime.UtcNow
            };

            await _formRepository.AddFormAsync(form);
            
            // Send notification
            await _notificationService.NotifyFormApplicationStatusAsync(
                formDto.StudentId, 
                "CompletionProgramme", 
                "Submitted");

            return new FormSubmissionDto
            {
                Id = 0, // Will be set by database
                SubmissionId = int.Parse(form.SubmissionId),
                StudentId = form.StudentId,
                FullName = form.FullName,
                Email = form.Email,
                Telephone = form.Telephone,
                PostalAddress = form.PostalAddress,
                FormType = form.FormType,
                Status = form.Status,
                EmailStatus = form.EmailStatus,
                SubmissionDate = form.SubmissionDate
            };
        }

        public async Task<FormSubmissionsResponseDto> GetStudentFormsAsync(string studentId)
        {
            _logger.LogInformation($"Retrieving forms for student {studentId}");
            
            var forms = await _formRepository.GetFormsByStudentIdAsync(studentId);
            var response = new FormSubmissionsResponseDto
            {
                Reconsideration = new List<FormSubmissionDto>(),
                CompassionateAegrotat = new List<FormSubmissionDto>(),
                CompletionProgramme = new List<FormSubmissionDto>()
            };

            foreach (var form in forms)
            {
                var formDto = new FormSubmissionDto
                {
                    Id = 0, // Will be set by database
                    SubmissionId = int.Parse(form.SubmissionId),
                    StudentId = form.StudentId,
                    FullName = form.FullName,
                    Email = form.Email,
                    Telephone = form.Telephone,
                    PostalAddress = form.PostalAddress,
                    FormType = form.FormType,
                    Status = form.Status,
                    EmailStatus = form.EmailStatus,
                    SubmissionDate = form.SubmissionDate
                };

                switch (form.FormType)
                {
                    case "Reconsideration":
                        response.Reconsideration.Add(formDto);
                        break;
                    case "CompassionateAegrotat":
                        response.CompassionateAegrotat.Add(formDto);
                        break;
                    case "CompletionProgramme":
                        response.CompletionProgramme.Add(formDto);
                        break;
                }
            }

            return response;
        }

        public async Task<bool> CheckEligibilityAsync(string studentId, string formType)
        {
            _logger.LogInformation($"Checking eligibility for {formType} for student {studentId}");
            
            // Get student record
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                return false;
            }

            switch (formType)
            {
                case "Reconsideration":
                    // Always eligible for reconsideration
                    return true;
                
                case "CompassionateAegrotat":
                    // Check if student has active enrollment
                    var enrollments = await _studentRepository.GetEnrollmentsAsync(studentId);
                    return enrollments.Any(e => e.Status == "Active");
                
                case "CompletionProgramme":
                    // Check if student has completed all required courses
                    var grades = await _gradeRepository.GetGradesByStudentIdAsync(studentId);
                    return grades.All(g => g.GradeValue != "F" && g.GradeValue != "I");
                
                default:
                    return false;
            }
        }
    }
}