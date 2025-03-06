using AutoMapper;
using BackendNotes.DTOs.TagsDTOs;
using BackendNotes.DTOs.NotesDTOs;
using BackendNotes.Entities;

namespace BackendNotes.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {


            CreateMap<NoteCreationDTO, Note>();
            CreateMap<Note, NoteDTO>()
            .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.Tags.Select(t => t.Id).ToList()));  // Map only Tag IDs


            CreateMap<TagCreationDTO, Tag>(); 
            CreateMap<Tag, TagDTO>();

            CreateMap<NoteUpdateDTO, Note>();
        }
    }
}
