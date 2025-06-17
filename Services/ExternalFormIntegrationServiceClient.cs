using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace Services
{
    public class ExternalFormIntegrationServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ExternalFormIntegrationServiceClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ServiceEndpoints:ExternalFormsService"] ?? "http://localhost:5003/api/external-forms";
        }

        public async Task<HttpResponseMessage> GetFormsAsync()
        {
            return await _httpClient.GetAsync($"{_baseUrl}/student");
        }

        public async Task<HttpResponseMessage> ApplyForFormAsync(string studentId, string formType, FormDataDto formData)
        {
            var request = new
            {
                StudentId = studentId,
                FormType = formType,
                FormData = formData
            };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync($"{_baseUrl}/submit?studentId={studentId}&formType={formType}", content);
        }

        public async Task<HttpResponseMessage> GetFormStatusAsync(string formId)
        {
            return await _httpClient.GetAsync($"{_baseUrl}/status/{formId}");
        }

        public async Task<HttpResponseMessage> UpdateFormStatusAsync(string formId, string status)
        {
            var content = new StringContent(JsonSerializer.Serialize(status), Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync($"{_baseUrl}/status/{formId}", content);
        }

        public async Task<HttpResponseMessage> CheckEligibilityAsync(string studentId, string formType)
        {
            return await _httpClient.GetAsync($"{_baseUrl}/eligibility?studentId={studentId}&formType={formType}");
        }

        public async Task<HttpResponseMessage> DownloadFormPdfAsync(string formId)
        {
            return await _httpClient.GetAsync($"{_baseUrl}/download/{formId}");
        }
    }
}