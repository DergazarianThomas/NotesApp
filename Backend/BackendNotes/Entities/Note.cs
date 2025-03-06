namespace BackendNotes.Entities
{

    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
