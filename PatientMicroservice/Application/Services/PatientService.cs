using PatientMicroservice.Application.DTOs;
using PatientMicroservice.Application.Interfaces;
using PatientMicroservice.Domain.Models;
using PatientMicroservice.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientMicroservice.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _repository.GetAllAsync();
            return patients.Select(p => ToDto(p)).ToList();
        }
        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            return patient == null ? null : ToDto(patient);
        }
        public async Task AddPatientAsync(PatientDto patientDto)
        {
            var patient = FromDto(patientDto);
            await _repository.AddAsync(patient);
        }
        public async Task UpdatePatientAsync(int id, PatientDto patientDto)
        {
            var patient = FromDto(patientDto);
            patient.Id = id; 
            await _repository.UpdateAsync(patient);
        }
        public async Task DeletePatientAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        private static PatientDto ToDto(Patient p) => new PatientDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            DateOfBirth = p.DateOfBirth,
            Gender = p.Gender,
            Adress = p.Adress,
            PhoneNumber = p.PhoneNumber,
        };

        private static Patient FromDto(PatientDto dto) => new Patient
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Adress = dto.Adress,
            PhoneNumber = dto.PhoneNumber,
        };
    }
}
