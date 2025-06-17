using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ExternalFormsService.Models;
using ExternalFormsService.Repositories;
using ExternalFormsService.DTOs;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ExternalFormsService.Services
{
    public class ExternalFormService : IExternalFormService
    {
        private readonly IExternalFormRepository _formRepository;
        private readonly ILogger<ExternalFormService> _logger;

        public ExternalFormService(
            IExternalFormRepository formRepository,
            ILogger<ExternalFormService> logger)
        {
            _formRepository = formRepository;
            _logger = logger;
        }

        public async Task<ExternalFormSubmissionDto> GetFormByIdAsync(string formId)
        {
            try
            {
                var form = await _formRepository.GetFormByIdAsync(formId);
                if (form == null)
                {
                    _logger.LogWarning("Form not found with ID: {FormId}", formId);
                    return null;
                }

                return MapToDto(form);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving form with ID: {FormId}", formId);
                throw;
            }
        }

        public async Task<IEnumerable<ExternalFormSubmissionDto>> GetStudentFormsAsync(string studentId)
        {
            try
            {
                var forms = await _formRepository.GetFormsByStudentIdAsync(studentId);
                return forms.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving forms for student: {StudentId}", studentId);
                throw;
            }
        }

        public async Task<ExternalFormSubmissionDto> SubmitFormAsync(string studentId, string formType, FormDataDto formData)
        {
            try
            {
                if (!await CheckEligibilityAsync(studentId, formType))
                {
                    throw new InvalidOperationException($"Student {studentId} is not eligible for form type {formType}");
                }

                var form = new FormSubmission
                {
                    StudentId = studentId,
                    FormType = formType,
                    FormData = formData.Fields,
                    Attachments = formData.Attachments,
                    Status = "Pending",
                    Comments = formData.Comments,
                    SubmissionDate = DateTime.UtcNow
                };

                await _formRepository.AddFormAsync(form);
                return MapToDto(form);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting form for student: {StudentId}", studentId);
                throw;
            }
        }

        public async Task<ExternalFormSubmissionDto> UpdateFormStatusAsync(string formId, string status, string comments, string updatedBy)
        {
            try
            {
                var form = await _formRepository.GetFormByIdAsync(formId);
                if (form == null)
                {
                    _logger.LogWarning("Form not found with ID: {FormId}", formId);
                    return null;
                }

                form.Status = status;
                form.Comments = comments;
                form.UpdatedBy = updatedBy;
                form.UpdatedAt = DateTime.UtcNow;

                await _formRepository.UpdateFormAsync(form);
                return MapToDto(form);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating form status for form: {FormId}", formId);
                throw;
            }
        }

        public async Task<bool> DeleteFormAsync(string formId)
        {
            try
            {
                return await _formRepository.DeleteFormAsync(formId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting form: {FormId}", formId);
                throw;
            }
        }

        public async Task<FormStatusDto> GetFormStatusAsync(string formId)
        {
            var form = await _formRepository.GetFormByIdAsync(formId);
            if (form == null)
            {
                throw new KeyNotFoundException($"Form with ID {formId} not found");
            }

            return new FormStatusDto
            {
                FormId = form.SubmissionId,
                Status = form.Status,
                LastUpdated = form.UpdatedAt ?? form.SubmissionDate
            };
        }

        public async Task<bool> NotifyFormStatusChangeAsync(string formId, string status)
        {
            var form = await _formRepository.GetFormByIdAsync(formId);
            if (form == null)
            {
                return false;
            }

            form.Status = status;
            form.UpdatedAt = DateTime.UtcNow;
            await _formRepository.UpdateFormAsync(form);

            return true;
        }

        public async Task<bool> CheckEligibilityAsync(string studentId, string formType)
        {
            try
            {
                // Add your eligibility checking logic here
                // For now, return true as a placeholder
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking eligibility for student: {StudentId}", studentId);
                throw;
            }
        }

        public async Task<byte[]> GenerateFormPdfAsync(string formId)
        {
            try
            {
                var form = await _formRepository.GetFormByIdAsync(formId);
                if (form == null)
                {
                    throw new InvalidOperationException($"Form not found with ID: {formId}");
                }

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.DefaultTextStyle(x => x.FontSize(10));

                        page.Header().Element(ComposeHeader);
                        page.Content().Element(x => ComposeContent(x, form));
                        page.Footer().Element(ComposeFooter);
                    });
                });

                return document.GeneratePdf();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF for form: {FormId}", formId);
                throw;
            }
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("External Form Submission").FontSize(20).SemiBold();
                    column.Item().Text(text =>
                    {
                        text.Span("Generated: ").SemiBold();
                        text.Span($"{DateTime.UtcNow:g}");
                    });
                });
            });
        }

        private void ComposeContent(IContainer container, FormSubmission form)
        {
            container.PaddingVertical(20).Column(column =>
            {
                column.Item().Text("Form Details").FontSize(16).SemiBold();
                column.Item().Text($"Form Type: {form.FormType}");
                column.Item().Text($"Student ID: {form.StudentId}");
                column.Item().Text($"Status: {form.Status}");
                column.Item().Text($"Submission Date: {form.SubmissionDate:g}");

                if (!string.IsNullOrEmpty(form.Comments))
                {
                    column.Item().Text("Comments:").SemiBold();
                    column.Item().Text(form.Comments);
                }

                column.Item().Text("Form Data:").SemiBold();
                foreach (var field in form.FormData)
                {
                    column.Item().Text($"{field.Key}: {field.Value}");
                }

                if (form.Attachments.Any())
                {
                    column.Item().Text("Attachments:").SemiBold();
                    foreach (var attachment in form.Attachments)
                    {
                        column.Item().Text($"- {attachment}");
                    }
                }
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Text(text =>
                {
                    text.Span("Page ");
                    text.CurrentPageNumber();
                    text.Span(" of ");
                    text.TotalPages();
                });
            });
        }

        private static ExternalFormSubmissionDto MapToDto(FormSubmission form)
        {
            return new ExternalFormSubmissionDto
            {
                Id = form.Id,
                StudentId = form.StudentId,
                FormType = form.FormType,
                FormData = form.FormData,
                Attachments = form.Attachments,
                Status = form.Status,
                Comments = form.Comments,
                SubmissionDate = form.SubmissionDate,
                UpdatedAt = form.UpdatedAt,
                UpdatedBy = form.UpdatedBy,
                EmailStatus = form.EmailStatus
            };
        }
    }
} 