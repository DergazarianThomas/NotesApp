using AutoMapper;
using BackendNotes.DTOs.NotesDTOs;
using BackendNotes.Entities;
using BackendNotes.Repositories;

namespace BackendNotes.Services
{
    public interface INoteService
    {
        Task<IEnumerable<NoteDTO>> GetAllNotesAsync();
        Task<NoteDTO> GetNoteByIdAsync(int id);
        Task<(IEnumerable<NoteDTO> Notes, int TotalCount, int TotalPages)> GetFilteredNotesAsync(
                bool? status, string searchTerm, List<int> tagIds, int page, int pageSize);
        Task<Note> CreateNoteAsync(NoteCreationDTO noteDTO);
        Task UpdateNoteAsync(int id, NoteUpdateDTO noteDTO);
        Task UpdateNoteStatusAsync(int id, NoteStatusUpdateDTO statusUpdate);
        Task DeleteNoteAsync(int id);
    }

    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public NoteService(INoteRepository noteRepository, ITagRepository tagRepository, IMapper mapper)
        {
            _noteRepository = noteRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NoteDTO>> GetAllNotesAsync()
        {
            var notes = await _noteRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<NoteDTO>>(notes);
        }

        public async Task<NoteDTO> GetNoteByIdAsync(int id)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            return _mapper.Map<NoteDTO>(note);
        }

        public async Task<(IEnumerable<NoteDTO> Notes, int TotalCount, int TotalPages)> GetFilteredNotesAsync(
            bool? status, string searchTerm, List<int> tagIds, int page, int pageSize)
        {

            var (notes, totalCount) = await _noteRepository.GetFilteredAsync(status, searchTerm, tagIds, page, pageSize);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return (_mapper.Map<IEnumerable<NoteDTO>>(notes), totalCount, totalPages);
        }

        public async Task<Note> CreateNoteAsync(NoteCreationDTO noteDTO)
        {
            var note = _mapper.Map<Note>(noteDTO);

            if (noteDTO.TagIds != null && noteDTO.TagIds.Any())
            {
                var tags = await _tagRepository.GetByIdsAsync(noteDTO.TagIds);
                note.Tags = tags.ToList();
            }

            return await _noteRepository.AddAsync(note);
        }

        public async Task UpdateNoteAsync(int id, NoteUpdateDTO noteDTO)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note == null)
            {
                throw new KeyNotFoundException($"Note with ID {id} not found");
            }

            _mapper.Map(noteDTO, note);

            if (noteDTO.TagIds != null)
            {
                note.Tags.Clear();

                if (noteDTO.TagIds.Any())
                {
                    var tags = await _tagRepository.GetByIdsAsync(noteDTO.TagIds);
                    foreach (var tag in tags)
                    {
                        note.Tags.Add(tag);
                    }
                }
            }

            await _noteRepository.UpdateAsync(note);
        }

        public async Task UpdateNoteStatusAsync(int id, NoteStatusUpdateDTO statusUpdate)
        {
            if (!await _noteRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"Note with ID {id} not found");
            }

            await _noteRepository.UpdateStatusAsync(id, statusUpdate.Status);
        }

        public async Task DeleteNoteAsync(int id)
        {
            if (!await _noteRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"Note with ID {id} not found");
            }

            await _noteRepository.DeleteAsync(id);
        }
    }
}
