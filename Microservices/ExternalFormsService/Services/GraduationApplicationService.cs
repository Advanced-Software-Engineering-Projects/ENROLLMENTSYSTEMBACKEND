using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ExternalFormsService.DTOs;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ExternalFormsService.Models;

namespace ExternalFormsService.Services
{
    public class GraduationApplicationService : IGraduationApplicationService
    {
        private readonly ILogger<GraduationApplicationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IExternalFormService _formService;

        public GraduationApplicationService(
            ILogger<GraduationApplicationService> logger,
            IConfiguration configuration,
            HttpClient httpClient,
            IExternalFormService formService)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
            _formService = formService;
        }

        public async Task<GraduationApplicationResponseDto> SubmitGraduationApplicationAsync(GraduationApplicationDto application)
        {
            try
            {
                // Validate student eligibility
                var isEligible = await ValidateStudentEligibility(application.StudentId, application.ApplicationType);

                // Create form submission
                var formSubmission = new FormSubmission
                {
                    StudentId = application.StudentId,
                    FormType = application.ApplicationType,
                    FormData = new Dictionary<string, string>
                    {
                        { "CourseIds", string.Join(",", application.CourseIds) },
                        { "Reason", application.Reason },
                        { "ApplicationDate", application.ApplicationDate.ToString() }
                    },
                    Attachments = application.SupportingDocuments != null ? new List<string>(application.SupportingDocuments.Values) : new List<string>(),
                    Status = "Pending",
                    Comments = application.Comments
                };

                var result = await _formService.SubmitFormAsync(
                    formSubmission.StudentId,
                    formSubmission.FormType,
                    new FormDataDto
                    {
                        FormType = formSubmission.FormType,
                        Fields = formSubmission.FormData,
                        Attachments = formSubmission.Attachments,
                        Status = formSubmission.Status,
                        Comments = formSubmission.Comments
                    });

                var response = new GraduationApplicationResponseDto
                {
                    ApplicationId = result.Id,
                    StudentId = application.StudentId,
                    ApplicationType = application.ApplicationType,
                    Status = isEligible ? "Pending" : "Rejected",
                    Message = isEligible ? "Application submitted successfully" : "Student is not eligible for this application",
                    IsQualified = isEligible,
                    ProcessedDate = DateTime.UtcNow
                };

                if (isEligible)
                {
                    await SendNotificationAsync(application.StudentId, response);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting graduation application for student: {StudentId}", application.StudentId);
                throw;
            }
        }

        public async Task<GraduationApplicationResponseDto> GetApplicationStatusAsync(string applicationId)
        {
            try
            {
                var form = await _formService.GetFormByIdAsync(applicationId);
                if (form == null)
                {
                    return null;
                }

                return new GraduationApplicationResponseDto
                {
                    ApplicationId = form.Id,
                    StudentId = form.StudentId,
                    ApplicationType = form.FormType,
                    Status = form.Status,
                    Message = form.Comments,
                    IsQualified = form.Status != "Rejected",
                    ProcessedDate = form.UpdatedAt ?? DateTime.MinValue
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving application status: {ApplicationId}", applicationId);
                throw;
            }
        }

        public async Task<bool> ProcessApplicationAsync(string applicationId, string status, string comments)
        {
            try
            {
                var result = await _formService.UpdateFormStatusAsync(
                    applicationId,
                    status,
                    comments,
                    "System");

                if (result != null)
                {
                    var response = new GraduationApplicationResponseDto
                    {
                        ApplicationId = result.Id,
                        StudentId = result.StudentId,
                        ApplicationType = result.FormType,
                        Status = status,
                        Message = comments,
                        IsQualified = status == "Approved",
                        ProcessedDate = DateTime.UtcNow
                    };

                    await SendNotificationAsync(result.StudentId, response);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing application: {ApplicationId}", applicationId);
                throw;
            }
        }

        public async Task<bool> SendNotificationAsync(string studentId, GraduationApplicationResponseDto response)
        {
            try
            {
                // Send email notification
                var emailEndpoint = _configuration["NotificationService:EmailEndpoint"];
                var emailContent = new
                {
                    To = studentId,
                    Subject = $"Graduation Application Update - {response.ApplicationType}",
                    Body = $"Your {response.ApplicationType} application has been {response.Status}. {response.Message}"
                };

                var emailResponse = await _httpClient.PostAsJsonAsync(emailEndpoint, emailContent);
                emailResponse.EnsureSuccessStatusCode();

                // Send system notification
                var notificationEndpoint = _configuration["NotificationService:NotificationEndpoint"];
                var notificationContent = new
                {
                    UserId = studentId,
                    Title = "Application Status Update",
                    Message = $"Your {response.ApplicationType} application has been {response.Status}",
                    Type = "ApplicationUpdate"
                };

                var notificationResponse = await _httpClient.PostAsJsonAsync(notificationEndpoint, notificationContent);
                notificationResponse.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification for student: {StudentId}", studentId);
                return false;
            }
        }

        private async Task<bool> ValidateStudentEligibility(string studentId, string applicationType)
        {
            try
            {
                var eligibilityEndpoint = _configuration["StudentService:EligibilityEndpoint"];
                var response = await _httpClient.GetAsync($"{eligibilityEndpoint}/{studentId}/{applicationType}");
                response.EnsureSuccessStatusCode();

                var eligibilityResult = await response.Content.ReadFromJsonAsync<bool>();
                return eligibilityResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating student eligibility: {StudentId}", studentId);
                return false;
            }
        }
    }
}