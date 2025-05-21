using PatientMicroservice.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientMicroservice.Infrastructure.Repositories
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(int id);

    }
}
