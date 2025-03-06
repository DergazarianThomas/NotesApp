using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendNotes;
using BackendNotes.Entities;
using BackendNotes.DTOs.NotesDTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendNotes.Services;

namespace BackendNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDTO>>> GetNotes()
        {
            var noteDTOs = await _noteService.GetAllNotesAsync();
            return Ok(noteDTOs);
        }

        [HttpGet("filtered")]
        public async Task<ActionResult<IEnumerable<NoteDTO>>> GetFilteredNotes(
            bool? status = null,
            string? searchTerm = null,
            [FromQuery] List<int>? tagIds = null,
            int page = 1,
            int pageSize = 10)
        {
            var result = await _noteService.GetFilteredNotesAsync(status, searchTerm, tagIds, page, pageSize);

            Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", result.TotalPages.ToString());

            return Ok(result.Notes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDTO>> GetNote(int id)
        {
            try
            {
                var noteDTO = await _noteService.GetNoteByIdAsync(id);
                return Ok(noteDTO);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote(int id, NoteUpdateDTO noteDTO)
        {
            try
            {
                await _noteService.UpdateNoteAsync(id, noteDTO);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("The note was modified by another user.");
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateNoteStatus(int id, [FromBody] NoteStatusUpdateDTO statusUpdate)
        {
            try
            {
                await _noteService.UpdateNoteStatusAsync(id, statusUpdate);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Note>> PostNote(NoteCreationDTO noteDTO)
        {
            var note = await _noteService.CreateNoteAsync(noteDTO);
            return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            try
            {
                await _noteService.DeleteNoteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
