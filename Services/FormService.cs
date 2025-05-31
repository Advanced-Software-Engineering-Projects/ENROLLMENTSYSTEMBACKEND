using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _formRepository;

        public FormService(IFormRepository formRepository)
        {
            _formRepository = formRepository;
        }

        public async Task<List<FormSubmissionDto>> GetFormsAsync(string? studentId, string? formType)
        {
            var forms = await _formRepository.GetFormsAsync(studentId, formType);
            return forms.Select(f => new FormSubmissionDto
            {
                SubmissionId = f.SubmissionId,
                StudentId = f.StudentId,
                FormType = f.FormType,
                Status = f.Status,
                SubmissionDate = f.SubmissionDate
            }).ToList();
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
            if (string.IsNullOrEmpty(formDto.StudentId) || string.IsNullOrEmpty(formDto.FormType))
            {
                throw new InvalidOperationException("Invalid form data.");
            }

            var form = new FormSubmission
            {
                StudentId = formDto.StudentId,
                FormType = formDto.FormType
            };

            await _formRepository.AddFormAsync(form);
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