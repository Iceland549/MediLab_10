using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RiskMicroservice.Application.DTOs;

namespace RiskMicroservice.Infrastructure.Clients
{
    public class NoteClient : INoteClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration; 

        public NoteClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<NoteDto>> GetNotesByPatientIdAsync(int patientId, string jwtToken)
        {
            var baseUrl = _configuration["NoteMicroserviceBaseUrl"];
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/api/notes?patientId={patientId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) 
                return new List<NoteDto>();
                var json = await response.Content.ReadAsStringAsync();
                var notes = JsonSerializer.Deserialize<List<NoteDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return notes ?? new List<NoteDto>();
        }
    }
}
