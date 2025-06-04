using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _formRepository;
        private readonly ExternalFormIntegrationServiceClient _externalFormClient;
        private readonly INotificationService _notificationService;

        public FormService(IFormRepository formRepository, ExternalFormIntegrationServiceClient externalFormClient, INotificationService notificationService)
        {
            _formRepository = formRepository;
            _externalFormClient = externalFormClient;
            _notificationService = notificationService;
        }

        public async Task<List<FormSubmissionDto>> GetFormsAsync(string? studentId, string? formType)
        {
            // For simplicity, call external form microservice for all forms
            var response = await _externalFormClient.GetFormsAsync();
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var forms = System.Text.Json.JsonSerializer.Deserialize<List<FormSubmissionDto>>(json);
            return forms;
        }

        public async Task<List<FormSubmission>> GetFormsAsync(string studentId)
        {
            return await _formRepository.GetFormsByStudentIdAsync(studentId);
        }

        public async Task<FormSubmissionDto> GetFormByIdAsync(string formId)
        {
            var form = await _formRepository.GetFormByIdAsync(formId);
            if (form == null)
            {
                return null;
            }
            return new FormSubmissionDto
            {
                SubmissionId = form.SubmissionId,
                StudentId = form.StudentId,
                FormType = form.FormType,
                Status = form.Status,
                SubmissionDate = form.SubmissionDate
            };
        }

        public async Task<FormSubmissionDto> CreateFormAsync(CreateFormDto createFormDto)
        {
            var form = new FormSubmission
            {
                StudentId = createFormDto.StudentId,
                FormType = createFormDto.FormType,
                Status = "Pending"
            };
            var createdForm = await _formRepository.CreateFormAsync(form);
            return new FormSubmissionDto
            {
                SubmissionId = createdForm.SubmissionId,
                StudentId = createdForm.StudentId,
                FormType = createdForm.FormType,
                Status = createdForm.Status,
                SubmissionDate = createdForm.SubmissionDate
            };
        }

        public async Task<FormSubmission> SubmitFormAsync(FormSubmissionDto formDto)
        {
            if (formDto == null)
            {
                throw new InvalidOperationException("Form data is required.");
            }
            if (string.IsNullOrEmpty(formDto.StudentId))
            {
                throw new InvalidOperationException("Student ID is required.");
            }
            if (string.IsNullOrEmpty(formDto.FormType))
            {
                throw new InvalidOperationException("Form type is required.");
            }

            var form = new FormSubmission
            {
                StudentId = formDto.StudentId,
                FormType = formDto.FormType,
                Status = "Submitted",
                SubmissionDate = DateTime.UtcNow
            };

            await _formRepository.AddFormAsync(form);

            // Notify student of form submission status
            await _notificationService.NotifyFormApplicationStatusAsync(formDto.StudentId, formDto.FormType, form.Status);

            return form;
        }

        public async Task<FormSubmissionDto> UpdateFormStatusAsync(UpdateStatusDto updateStatusDto)
        {
            var form = await _formRepository.GetFormByIdAsync(updateStatusDto.SubmissionId);
            if (form == null)
            {
                return null;
            }
            await _formRepository.UpdateFormStatusAsync(updateStatusDto.SubmissionId, updateStatusDto.Status);
            form.Status = updateStatusDto.Status;

            // Notify student of updated form status
            await _notificationService.NotifyFormApplicationStatusAsync(form.StudentId, form.FormType, form.Status);

            return new FormSubmissionDto
            {
                SubmissionId = form.SubmissionId,
                StudentId = form.StudentId,
                FormType = form.FormType,
                Status = form.Status,
                SubmissionDate = form.SubmissionDate
            };
        }
    }
}
