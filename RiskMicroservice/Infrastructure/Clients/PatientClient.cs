using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RiskMicroservice.Application.DTOs;

namespace RiskMicroservice.Infrastructure.Clients
{
    public class PatientClient : IPatientClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PatientClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int patientId, string jwtToken)
        {
            var baseUrl = _configuration["PatientMicroserviceBaseUrl"];
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/api/patient/{patientId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PatientDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
