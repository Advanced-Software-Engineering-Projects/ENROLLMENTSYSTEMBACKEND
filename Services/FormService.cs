using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _formRepository;
        private readonly IFormSubmissionRepository _formSubmissionRepository;
        private readonly IFormConfigurationService _formConfigurationService;

        public FormService(
            IFormRepository formRepository,
            IFormSubmissionRepository formSubmissionRepository,
            IFormConfigurationService formConfigurationService)
        {
            _formRepository = formRepository;
            _formSubmissionRepository = formSubmissionRepository;
            _formConfigurationService = formConfigurationService;
        }

        public async Task<List<FormSubmissionDto>> GetFormsAsync(string? studentId, string? formType)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                throw new InvalidOperationException("Student ID cannot be null or empty.");
            }

            var forms = await _formSubmissionRepository.GetFormSubmissionsByStudentIdAsync(studentId);
            if (forms == null || !forms.Any())
            {
                return new List<FormSubmissionDto>();
            }
            
            if (!string.IsNullOrEmpty(formType))
                forms = forms.FindAll(f => f.FormType.Equals(formType, StringComparison.OrdinalIgnoreCase));
            
            var formDtos = new List<FormSubmissionDto>();
            foreach (var form in forms)
            {
                formDtos.Add(_formConfigurationService.MapToFormSubmissionDto(form));
            }
            
            return formDtos;
        }

        public async Task<FormSubmissionDto> GetFormByIdAsync(string formId)
        {
            var form = await _formSubmissionRepository.GetFormSubmissionByIdAsync(formId);
            if (form == null)
                throw new InvalidOperationException($"Form with ID {formId} not found");
                
            return _formConfigurationService.MapToFormSubmissionDto(form);
        }

        public async Task<FormSubmissionDto> CreateFormAsync(CreateFormDto createFormDto)
        {
            var formSubmission = new FormSubmission
            {
                SubmissionId = Guid.NewGuid().ToString(),
                StudentId = createFormDto.StudentId,
                FormType = createFormDto.FormType,
                Status = "Pending",
                EmailStatus = "Pending",
                SubmissionDate = DateTime.UtcNow
            };
            
            await _formSubmissionRepository.CreateFormSubmissionAsync(formSubmission);
            return _formConfigurationService.MapToFormSubmissionDto(formSubmission);
        }

        public async Task<FormSubmissionDto> UpdateFormStatusAsync(UpdateStatusDto updateStatusDto)
        {
            var form = await _formSubmissionRepository.GetFormSubmissionByIdAsync(updateStatusDto.SubmissionId);
            if (form == null)
                throw new InvalidOperationException($"Form with ID {updateStatusDto.SubmissionId} not found");
                
            form.Status = updateStatusDto.Status;
            await _formSubmissionRepository.UpdateFormSubmissionAsync(form);
            
            return _formConfigurationService.MapToFormSubmissionDto(form);
        }

        public async Task<List<FormSubmission>> GetFormsAsync(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                throw new ArgumentException("Student ID cannot be null or empty", nameof(studentId));
            }

            var forms = await _formSubmissionRepository.GetFormSubmissionsByStudentIdAsync(studentId);
            return forms ?? new List<FormSubmission>();
        }

        public async Task<FormSubmission> SubmitFormAsync(FormSubmissionDto formDto)
        {
            var form = new FormSubmission
            {
                SubmissionId = Guid.NewGuid().ToString(),
                StudentId = formDto.StudentId,
                FullName = formDto.FullName,
                Email = formDto.Email,
                Telephone = formDto.Telephone,
                PostalAddress = formDto.PostalAddress,
                FormType = formDto.FormType,
                Status = "Pending",
                EmailStatus = "Pending",
                SubmissionDate = DateTime.UtcNow,
                DateOfBirth = formDto.DateOfBirth,
                Sponsorship = formDto.Sponsorship,
                CourseCode = formDto.CourseCode,
                CourseLecturer = formDto.CourseLecturer,
                CourseTitle = formDto.CourseTitle,
                ReceiptNo = formDto.ReceiptNo,
                PaymentConfirmation = formDto.PaymentConfirmation,
                CurrentGrade = formDto.CurrentGrade,
                Campus = formDto.Campus,
                Semester = formDto.Semester,
                Year = formDto.Year,
                Reason = formDto.Reason,
                SupportingDocuments = formDto.SupportingDocuments,
                ApplicantSignature = formDto.ApplicantSignature,
                Date = formDto.Date,
                Programme = formDto.Programme,
                ExamDate = formDto.ExamDate,
                ExamStartTime = formDto.ExamStartTime,
                ApplyingFor = formDto.ApplyingFor
            };

            await _formSubmissionRepository.CreateFormSubmissionAsync(form);
            await _formConfigurationService.SendFormSubmissionEmailAsync(formDto.FormType, form.SubmissionId);

            return form;
        }

        public async Task<FormSubmissionDto?> GetFormSubmissionByIdAsync(string submissionId)
        {
            var form = await _formSubmissionRepository.GetFormSubmissionByIdAsync(submissionId);
            if (form == null)
                return null;

            return _formConfigurationService.MapToFormSubmissionDto(form);
        }

        public async Task<List<FormSubmissionDto>> GetFormSubmissionsByStudentIdAsync(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                throw new ArgumentException("Student ID cannot be null or empty", nameof(studentId));
            }

            var forms = await _formSubmissionRepository.GetFormSubmissionsByStudentIdAsync(studentId);
            if (forms == null || !forms.Any())
            {
                return new List<FormSubmissionDto>();
            }

            var formDtos = new List<FormSubmissionDto>();
            foreach (var form in forms)
            {
                formDtos.Add(_formConfigurationService.MapToFormSubmissionDto(form));
            }

            return formDtos;
        }

        public async Task<bool> CreateFormSubmissionAsync(FormSubmissionDto formDto)
        {
            var form = new FormSubmission
            {
                SubmissionId = Guid.NewGuid().ToString(),
                StudentId = formDto.StudentId,
                FullName = formDto.FullName,
                Email = formDto.Email,
                Telephone = formDto.Telephone,
                PostalAddress = formDto.PostalAddress,
                FormType = formDto.FormType,
                Status = "Pending",
                EmailStatus = "Pending",
                SubmissionDate = DateTime.UtcNow,
                DateOfBirth = formDto.DateOfBirth,
                Sponsorship = formDto.Sponsorship,
                CourseCode = formDto.CourseCode,
                CourseLecturer = formDto.CourseLecturer,
                CourseTitle = formDto.CourseTitle,
                ReceiptNo = formDto.ReceiptNo,
                PaymentConfirmation = formDto.PaymentConfirmation,
                CurrentGrade = formDto.CurrentGrade,
                Campus = formDto.Campus,
                Semester = formDto.Semester,
                Year = formDto.Year,
                Reason = formDto.Reason,
                SupportingDocuments = formDto.SupportingDocuments,
                ApplicantSignature = formDto.ApplicantSignature,
                Date = formDto.Date,
                Programme = formDto.Programme,
                ExamDate = formDto.ExamDate,
                ExamStartTime = formDto.ExamStartTime,
                ApplyingFor = formDto.ApplyingFor
            };

            var result = await _formSubmissionRepository.CreateFormSubmissionAsync(form);
            await _formConfigurationService.SendFormSubmissionEmailAsync(formDto.FormType, form.SubmissionId);

            return result;
        }
    }
}