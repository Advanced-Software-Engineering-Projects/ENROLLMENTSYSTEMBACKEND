using ENROLLMENTSYSTEMBACKEND.DTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class HoldManagementServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5003/api";
        private readonly JsonSerializerOptions _jsonOptions;

        public HoldManagementServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<HttpResponseMessage> GetHoldsAsync(string studentId)
        {
            return await _httpClient.GetAsync($"{_baseUrl}/HoldManagement/student-holds/student/{studentId}");
        }

        public async Task<HttpResponseMessage> CheckServiceAccessAsync(string studentId, string service)
        {
            return await _httpClient.GetAsync($"{_baseUrl}/HoldManagement/integration/check-access?studentId={studentId}&service={service}&apiKey=your-secure-api-key");
        }

        public async Task<HttpResponseMessage> AddHoldAsync(string studentId, string service, string reason)
        {
            var hold = new
            {
                StudentId = studentId,
                HoldType = "FeeNonPayment",
                Reason = reason
            };

            var content = new StringContent(JsonSerializer.Serialize(hold), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync($"{_baseUrl}/HoldManagement/student-holds", content);
        }

        public async Task<HttpResponseMessage> RemoveHoldAsync(string holdId)
        {
            return await _httpClient.DeleteAsync($"{_baseUrl}/HoldManagement/student-holds/{holdId}");
        }

        public async Task<HttpResponseMessage> GetServiceAccessRulesAsync()
        {
            return await _httpClient.GetAsync($"{_baseUrl}/HoldManagement/service-access-rules");
        }

        public async Task<HttpResponseMessage> UpdateServiceAccessRulesAsync(Dictionary<string, bool> allowedServices)
        {
            var rules = new
            {
                HoldType = "FeeNonPayment",
                AllowedServices = allowedServices,
                UpdatedBy = "Admin"
            };

            var content = new StringContent(JsonSerializer.Serialize(rules), Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync($"{_baseUrl}/HoldManagement/service-access-rules", content);
        }
    }
}