using BackendNotes.DTOs.NotesDTOs;

namespace BackendNotes.DTOs.TagsDTOs
{
    public class TagDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<NoteDTO>? Notes { get; set; }
    }
}
