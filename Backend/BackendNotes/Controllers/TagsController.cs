using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendNotes;
using BackendNotes.Entities;
using BackendNotes.DTOs.TagsDTOs;
using AutoMapper;
using BackendNotes.DTOs.NotesDTOs;
using BackendNotes.Services;

namespace BackendNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return tag;
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> PostTag(TagCreationDTO tagDTO)
        {
            var tag = await _tagService.CreateTagAsync(tagDTO);
            return CreatedAtAction("GetTag", new { id = tag.Id }, tag);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var result = await _tagService.DeleteTagAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
