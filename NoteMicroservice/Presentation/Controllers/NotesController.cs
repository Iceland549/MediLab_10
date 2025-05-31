using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteMicroservice.Application.DTOs;
using NoteMicroservice.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteMicroservice.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<NoteDto>>> GetNotesByPatientId([FromQuery] int patientId)
        {
            var notes = await _noteService.GetNotesByPatientIdAsync(patientId);
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDto>> GetById(string id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null)
                return NotFound();
            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NoteDto noteDto)
        {
            try
            {
                // Ajouter la note et récupérer l'ID généré
                var generatedId = await _noteService.AddNoteAsync(noteDto);

                // Mettre à jour noteDto avec l'ID généré
                noteDto.Id = generatedId;

                return CreatedAtAction(nameof(GetById), new { id = noteDto.Id }, noteDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de l'ajout de la note : {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] NoteDto noteDto)
        {
            var existing = await _noteService.GetNoteByIdAsync(id);
            if (existing == null)
                return NotFound();
            await _noteService.UpdateNoteAsync(id, noteDto);
            var updatedNote = await _noteService.GetNoteByIdAsync(id);
            return Ok(updatedNote);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var existing = await _noteService.GetNoteByIdAsync(id);
                if (existing == null)
                    return NotFound();
                await _noteService.DeleteNoteAsync(id);
                return NoContent();
            }
            catch (FormatException)
            {
                return BadRequest("L'ID fourni n'est pas un ObjectId valide.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la suppression : {ex.Message}");
            }
        }
    }
}
