using ENROLLMENTSYSTEMBACKEND.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class StudentFormsServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StudentFormsServiceClient> _logger;
        private readonly string _baseUrl;

        public StudentFormsServiceClient(
            HttpClient httpClient,
            ILogger<StudentFormsServiceClient> logger,
            string baseUrl)
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseUrl = baseUrl;
        }

        public async Task<HttpResponseMessage> GetStudentFormsAsync(string studentId)
        {
            try
            {
                return await _httpClient.GetAsync($"forms?studentId={studentId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving forms for student {studentId}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> CheckEligibilityAsync(string studentId, string formType)
        {
            try
            {
                return await _httpClient.GetAsync($"{_baseUrl}forms/eligibility?studentId={studentId}&formType={formType}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking eligibility for student {studentId} and form type {formType}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> SubmitReconsiderationFormAsync(ReconsiderationFormDto formDto)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(formDto),
                    Encoding.UTF8,
                    "application/json");
                
                return await _httpClient.PostAsync($"{_baseUrl}forms/reconsideration", content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error submitting reconsideration form for student {formDto.StudentId}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> SubmitCompassionateAegrotatFormAsync(CompassionateAegrotatFormDto formDto)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(formDto),
                    Encoding.UTF8,
                    "application/json");
                
                return await _httpClient.PostAsync($"{_baseUrl}forms/compassionate-aegrotat", content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error submitting compassionate/aegrotat form for student {formDto.StudentId}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> SubmitCompletionProgrammeFormAsync(CompletionProgrammeFormDto formDto)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(formDto),
                    Encoding.UTF8,
                    "application/json");
                
                return await _httpClient.PostAsync($"{_baseUrl}forms/completion-programme", content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error submitting completion programme form for student {formDto.StudentId}");
                throw;
            }
        }
    }
}