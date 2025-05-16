using PatientMicroservice.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientMicroservice.Application.Interfaces
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllPatientsAsync();
        Task<PatientDto> GetPatientByIdAsync(int id);
        Task AddPatientAsync(PatientDto patientDto);
        Task UpdatePatientAsync(int id, PatientDto patientDto);
        Task DeletePatientAsync(int id);
    }
}
