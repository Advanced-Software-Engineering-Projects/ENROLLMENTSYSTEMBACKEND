using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class ExternalFormIntegrationServiceClient
    {
        private readonly HttpClient _httpClient;

        public ExternalFormIntegrationServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public ExternalFormIntegrationServiceClient()
        {
        }

        public async Task<HttpResponseMessage> GetFormsAsync()
        {
            return await _httpClient.GetAsync("api/externalforms/forms");
        }

        public async Task<HttpResponseMessage> ApplyForFormAsync(string studentId, string formType)
        {
            var request = new
            {
                StudentId = studentId,
                FormType = formType
            };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("api/externalforms/apply", content);
        }
    }
}
