using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class FormConfigurationService : IFormConfigurationService
    {
        private readonly IFormRepository _formRepository;
        private readonly IFormSubmissionRepository _formSubmissionRepository;
        private readonly IEmailService _emailService;

        public FormConfigurationService(
            IFormRepository formRepository,
            IFormSubmissionRepository formSubmissionRepository,
            IEmailService emailService)
        {
            _formRepository = formRepository;
            _formSubmissionRepository = formSubmissionRepository;
            _emailService = emailService;
        }

        public async Task<FormSubmissionsResponseDto> GetAllFormSubmissionsAsync()
        {
            var forms = await _formSubmissionRepository.GetFormSubmissionsByStudentIdAsync(null);
            var formDtos = new List<FormSubmissionDto>();

            foreach (var form in forms)
            {
                formDtos.Add(MapToFormSubmissionDto(form));
            }

            return new FormSubmissionsResponseDto { Forms = formDtos };
        }

        public async Task<bool> UpdateGradeAsync(GradeUpdateDto gradeUpdateDto)
        {
            var form = await _formSubmissionRepository.GetFormSubmissionByIdAsync(gradeUpdateDto.SubmissionId.ToString());
            if (form == null)
                return false;

            form.NewGrade = gradeUpdateDto.NewGrade;
            return await _formSubmissionRepository.UpdateFormSubmissionAsync(form);
        }

        public async Task<bool> UpdateStatusAsync(StatusUpdateDto statusUpdateDto, string formType)
        {
            var form = await _formSubmissionRepository.GetFormSubmissionByIdAsync(statusUpdateDto.SubmissionId.ToString());
            if (form == null)
                return false;

            form.Status = statusUpdateDto.Status;
            return await _formSubmissionRepository.UpdateFormSubmissionAsync(form);
        }

        public async Task<bool> ResendEmailAsync(EmailResendDto emailResendDto)
        {
            return await ResendFormSubmissionEmailAsync(emailResendDto.FormType, emailResendDto.SubmissionId.ToString(), emailResendDto.StudentEmail ?? string.Empty);
        }

        public async Task<bool> SendFormSubmissionEmailAsync(string formType, string submissionId)
        {
            try
            {
                switch (formType.ToLower())
                {
                    case "reconsideration":
                        return await SendReconsiderationFormEmailAsync(submissionId);
                    case "compassionateaegrotat":
                        return await SendCompassionateAegrotatFormEmailAsync(submissionId);
                    case "completionprogramme":
                        return await SendCompletionProgrammeFormEmailAsync(submissionId);
                    default:
                        throw new ArgumentException($"Unknown form type: {formType}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error sending form submission email: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> SendReconsiderationFormEmailAsync(string submissionId)
        {
            var form = await _formSubmissionRepository.GetFormSubmissionByIdAsync(submissionId);
            if (form == null)
                return false;

            var emailBody = $@"
                <h2>Reconsideration Form Submission</h2>
                <p>Dear {form.FullName},</p>
                <p>Your reconsideration form has been submitted successfully.</p>
                <p>Submission ID: {submissionId}</p>
                <p>Course: {form.CourseCode}</p>
                <p>Status: {form.Status}</p>
                <p>Thank you for your submission.</p>
            ";

            return await _emailService.SendEmailAsync(form.Email, "Reconsideration Form Submission", emailBody);
        }

        private async Task<bool> SendCompassionateAegrotatFormEmailAsync(string submissionId)
        {
            var form = await _formSubmissionRepository.GetFormSubmissionByIdAsync(submissionId);
            if (form == null)
                return false;

            var emailBody = $@"
                <h2>Compassionate/Aegrotat Form Submission</h2>
                <p>Dear {form.FullName},</p>
                <p>Your compassionate/aegrotat form has been submitted successfully.</p>
                <p>Submission ID: {submissionId}</p>
                <p>Course: {form.CourseCode}</p>
                <p>Status: {form.Status}</p>
                <p>Thank you for your submission.</p>
            ";

            return await _emailService.SendEmailAsync(form.Email, "Compassionate/Aegrotat Form Submission", emailBody);
        }

        private async Task<bool> SendCompletionProgrammeFormEmailAsync(string submissionId)
        {
            var form = await _formSubmissionRepository.GetFormSubmissionByIdAsync(submissionId);
            if (form == null)
                return false;

            var emailBody = $@"
                <h2>Completion Programme Form Submission</h2>
                <p>Dear {form.FullName},</p>
                <p>Your completion programme form has been submitted successfully.</p>
                <p>Submission ID: {submissionId}</p>
                <p>Programme: {form.Programme}</p>
                <p>Status: {form.Status}</p>
                <p>Thank you for your submission.</p>
            ";

            return await _emailService.SendEmailAsync(form.Email, "Completion Programme Form Submission", emailBody);
        }

        public async Task<bool> ResendFormSubmissionEmailAsync(string formType, string submissionId, string email)
        {
            try
            {
                switch (formType.ToLower())
                {
                    case "reconsideration":
                        return await ResendReconsiderationFormEmailAsync(submissionId, email);
                    case "compassionateaegrotat":
                        return await ResendCompassionateAegrotatFormEmailAsync(submissionId, email);
                    case "completionprogramme":
                        return await ResendCompletionProgrammeFormEmailAsync(submissionId, email);
                    default:
                        throw new ArgumentException($"Unknown form type: {formType}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error resending form submission email: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ResendReconsiderationFormEmailAsync(string submissionId, string email)
        {
            var form = await _formSubmissionRepository.GetFormSubmissionByIdAsync(submissionId);
            if (form == null)
                return false;

            var emailBody = $@"
                <h2>Reconsideration Form Submission</h2>
                <p>Dear {form.FullName},</p>
                <p>Your reconsideration form has been submitted successfully.</p>
                <p>Submission ID: {submissionId}</p>
                <p>Course: {form.CourseCode}</p>
                <p>Status: {form.Status}</p>
                <p>Thank you for your submission.</p>
            ";

            return await _emailService.SendEmailAsync(email, "Reconsideration Form Submission", emailBody);
        }

        private async Task<bool> ResendCompassionateAegrotatFormEmailAsync(string submissionId, string email)
        {
            var form = await _formSubmissionRepository.GetFormSubmissionByIdAsync(submissionId);
            if (form == null)
                return false;

            var emailBody = $@"
                <h2>Compassionate/Aegrotat Form Submission</h2>
                <p>Dear {form.FullName},</p>
                <p>Your compassionate/aegrotat form has been submitted successfully.</p>
                <p>Submission ID: {submissionId}</p>
                <p>Course: {form.CourseCode}</p>
                <p>Status: {form.Status}</p>
                <p>Thank you for your submission.</p>
            ";

            return await _emailService.SendEmailAsync(email, "Compassionate/Aegrotat Form Submission", emailBody);
        }

        private async Task<bool> ResendCompletionProgrammeFormEmailAsync(string submissionId, string email)
        {
            var form = await _formSubmissionRepository.GetFormSubmissionByIdAsync(submissionId);
            if (form == null)
                return false;

            var emailBody = $@"
                <h2>Completion Programme Form Submission</h2>
                <p>Dear {form.FullName},</p>
                <p>Your completion programme form has been submitted successfully.</p>
                <p>Submission ID: {submissionId}</p>
                <p>Programme: {form.Programme}</p>
                <p>Status: {form.Status}</p>
                <p>Thank you for your submission.</p>
            ";

            return await _emailService.SendEmailAsync(email, "Completion Programme Form Submission", emailBody);
        }

        public FormSubmissionDto MapToFormSubmissionDto(FormSubmission form)
        {
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
                DateOfBirth = form.DateOfBirth,
                Sponsorship = form.Sponsorship,
                CourseCode = form.CourseCode,
                CourseLecturer = form.CourseLecturer,
                CourseTitle = form.CourseTitle,
                ReceiptNo = form.ReceiptNo,
                PaymentConfirmation = form.PaymentConfirmation,
                CurrentGrade = form.CurrentGrade,
                Campus = form.Campus,
                Semester = form.Semester,
                Year = form.Year,
                Reason = form.Reason,
                SupportingDocuments = form.SupportingDocuments,
                ApplicantSignature = form.ApplicantSignature,
                Date = form.Date,
                Programme = form.Programme,
                ExamDate = form.ExamDate,
                ExamStartTime = form.ExamStartTime,
                ApplyingFor = form.ApplyingFor,
                NewGrade = form.NewGrade
            };
        }
    }
}