using System.ComponentModel.DataAnnotations;

namespace BackendNotes.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Note> Notes { get; set; }
    }
}
