using AutoMapper;
using BackendNotes.DTOs.TagsDTOs;
using BackendNotes.Entities;
using BackendNotes.Repositories;

namespace BackendNotes.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDTO>> GetAllTagsAsync();
        Task<Tag> GetTagByIdAsync(int id);
        Task<Tag> CreateTagAsync(TagCreationDTO tagDTO);
        Task<bool> DeleteTagAsync(int id);
    }

    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagService(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagDTO>> GetAllTagsAsync()
        {
            var tags = await _tagRepository.GetAllTagsAsync(includeNotes: true);
            return _mapper.Map<IEnumerable<TagDTO>>(tags);
        }

        public async Task<Tag> GetTagByIdAsync(int id)
        {
            return await _tagRepository.GetTagByIdAsync(id);
        }

        public async Task<Tag> CreateTagAsync(TagCreationDTO tagDTO)
        {
            var tag = _mapper.Map<Tag>(tagDTO);
            return await _tagRepository.CreateTagAsync(tag);
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            return await _tagRepository.DeleteTagAsync(id);
        }
    }
}
