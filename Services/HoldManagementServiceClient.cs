using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class HoldManagementServiceClient
    {
        private readonly HttpClient _httpClient;

        public HoldManagementServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetHoldsAsync(string studentId)
        {
            return await _httpClient.GetAsync($"api/hold/{studentId}");
        }

        public async Task<HttpResponseMessage> AddHoldAsync(string studentId, string service, string reason)
        {
            var request = new
            {
                StudentId = studentId,
                Service = service,
                Reason = reason
            };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("api/hold", content);
        }

        public async Task<HttpResponseMessage> RemoveHoldAsync(string holdId)
        {
            return await _httpClient.DeleteAsync($"api/hold/{holdId}");
        }
    }
}
