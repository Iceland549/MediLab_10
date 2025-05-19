using NoteMicroservice.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteMicroservice.Application.Interfaces
{
    public interface INoteService
    {
        Task<List<NoteDto>> GetNotesByPatientIdAsync(int patientId);
        Task<NoteDto> GetNoteByIdAsync(string id);
        Task AddNoteAsync(NoteDto noteDto);
        Task UpdateNoteAsync(string id, NoteDto noteDto);
        Task DeleteNoteAsync(string id);
    }
}
