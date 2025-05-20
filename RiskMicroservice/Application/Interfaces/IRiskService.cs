using System.Threading.Tasks;
using RiskMicroservice.Application.DTOs;

namespace RiskMicroservice.Application.Interfaces
{
    public interface IRiskService
    {
        Task<RiskAssessmentDto?> AssessmentRiskAsync(int patientId, string jwtToken);
    }
}
