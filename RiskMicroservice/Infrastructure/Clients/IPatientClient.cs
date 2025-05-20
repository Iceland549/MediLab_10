using System.Threading.Tasks;
using RiskMicroservice.Application.DTOs;

namespace RiskMicroservice.Infrastructure.Clients
{
    public interface IPatientClient
    {
        Task<PatientDto?> GetPatientByIdAsync(int patientId, string jwtToken);
    }
}
