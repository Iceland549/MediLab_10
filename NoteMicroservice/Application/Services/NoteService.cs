using NoteMicroservice.Application.DTOs;
using NoteMicroservice.Application.Interfaces;
using NoteMicroservice.Domain.Models;
using NoteMicroservice.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteMicroservice.Application.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _repository;

        public NoteService(INoteRepository repository)
        { _repository = repository; }
        public async Task<List<NoteDto>> GetNotesByPatientIdAsync(int patientId)
        {
            var notes = await _repository.GetNotesByPatientIdAsync(patientId);
            return notes.Select(n => ToDto(n)).ToList();
        }
        public async Task<NoteDto?> GetNoteByIdAsync(string id)
        {
            var note = await _repository.GetByIdAsync(id);
            return note == null ? null : ToDto(note);
        }
        public async Task<string> AddNoteAsync(NoteDto noteDto)
        {
            var note = FromDto(noteDto);
            await _repository.AddAsync(note);
            return note.Id;
        }
        public async Task UpdateNoteAsync(string id, NoteDto noteDto)
        {
            if (noteDto == null)
                throw new ArgumentNullException(nameof(noteDto));
            var existingNote = await _repository.GetByIdAsync(id);

            if (!string.IsNullOrEmpty(noteDto.Content))
                existingNote.Content = noteDto.Content;

            await _repository.UpdateAsync(existingNote);
        }
        public async Task DeleteNoteAsync(string id)
        {
            var objectId = MongoDB.Bson.ObjectId.Parse(id);
            await _repository.DeleteAsync(objectId.ToString());
        }

        private static NoteDto ToDto(Note note) => new NoteDto
        {
            Id = note.Id,
            PatientId = note.PatientId,
            Content = note.Content,
            DateCreated = note.DateCreated,
        };

        private static Note FromDto(NoteDto dto) => new Note
        {
            Id = dto.Id,
            PatientId = dto.PatientId,
            Content = dto.Content,
            DateCreated = dto.DateCreated,
        };
    }
}
