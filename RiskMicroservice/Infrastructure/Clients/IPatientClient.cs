using System.Threading.Tasks;

namespace RiskMicroservice.Infrastructure.Clients
{
    public interface IPatientClient
    {
        Task GetPatientByIdAsync(int patientId, string jwtToken);
    }
}
