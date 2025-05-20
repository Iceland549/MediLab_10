using RiskMicroservice.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RiskMicroservice.Infrastructure.Clients
{
    public interface INoteClient
    {
        Task<NoteDto?> GetNotesByPatientIdAsync(int patientId, string jwtToken);
    }
}
