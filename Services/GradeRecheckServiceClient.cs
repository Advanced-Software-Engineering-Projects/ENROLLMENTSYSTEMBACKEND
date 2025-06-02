using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class GradeRecheckServiceClient
    {
        private readonly HttpClient _httpClient;

        public GradeRecheckServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> ApplyGradeRecheckAsync(string studentId, string courseId, string reason)
        {
            var request = new
            {
                StudentId = studentId,
                CourseId = courseId,
                Reason = reason
            };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("api/graderecheck/apply", content);
        }

        public async Task<HttpResponseMessage> GetApplicationStatusAsync(string studentId, string courseId)
        {
            return await _httpClient.GetAsync($"api/graderecheck/status/{studentId}/{courseId}");
        }
    }
}
