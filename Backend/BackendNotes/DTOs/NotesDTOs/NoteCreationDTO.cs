using BackendNotes.Entities;

namespace BackendNotes.DTOs.NotesDTOs
{
    public class NoteCreationDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
        public List<int>? TagIds { get; set; }
    }
}
