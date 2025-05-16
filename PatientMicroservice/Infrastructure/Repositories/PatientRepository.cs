using Microsoft.EntityFrameworkCore;
using PatientMicroservice.Domain.Models;
using PatientMicroservice.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientMicroservice.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {   
        private readonly PatientDbContext _context;

        public PatientRepository(PatientDbContext context)
        {
            _context = context;
        }

        public async Task<List<Patient>> GetAllAsync()
        {
            return await _context.Patients.ToListAsync();
        }
        public async Task<Patient> GetByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }
        public async Task AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }
    }
}
