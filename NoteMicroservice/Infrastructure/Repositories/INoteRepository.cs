using NoteMicroservice.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteMicroservice.Infrastructure.Repositories
{
    public interface INoteRepository
    {
        Task<List<Note>> GetNotesByPatientIdAsync(int patientId);
        Task<Note> GetByIdAsync(string id);
        Task AddAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(string id);
    }
}
