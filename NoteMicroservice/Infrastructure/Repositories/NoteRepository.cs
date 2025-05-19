using MongoDB.Driver;
using NoteMicroservice.Domain.Models;
using NoteMicroservice.Infrastructure.Data;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteMicroservice.Infrastructure.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly IMongoCollection<Note> _notesCollection;

        public NoteRepository(IOptions<MongoDbConfig> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(config.Value.DatabaseName);
            _notesCollection = mongoDatabase.GetCollection<Note>(config.Value.NotesCollectionName);
        }

        public async Task<List<Note>> GetNotesByPatientIdAsync(int patientId)
        {
            return await _notesCollection.Find(n => n.PatientId == patientId).ToListAsync(); 
        }

        public async Task<Note> GetByIdAsync(string id)
        {
            return await _notesCollection.Find(n => n.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Note note)
        {
            await _notesCollection.InsertOneAsync(note);
        }

        public async Task UpdateAsync(Note note)
        {
            await _notesCollection.ReplaceOneAsync(n => n.Id == note.Id, note);
        }

        public async Task DeleteAsync(string id)
        {
            await _notesCollection.DeleteOneAsync(n => n.Id == id);
        }
    }
}
