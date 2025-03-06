using BackendNotes.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendNotes.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllAsync();
        Task<Note> GetByIdAsync(int id);
        Task<(IEnumerable<Note> notes, int totalCount)> GetFilteredAsync(bool? status, string searchTerm, List<int> tagIds, int page, int pageSize);
        Task<Note> AddAsync(Note note);
        Task UpdateAsync(Note note);
        Task UpdateStatusAsync(int id, bool status);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }

    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext _context;

        public NoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Note>> GetAllAsync()
        {
            return await _context.Notes
                .Include(n => n.Tags)
                .ToListAsync();
        }

        public async Task<Note> GetByIdAsync(int id)
        {
            return await _context.Notes
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<(IEnumerable<Note> notes, int totalCount)> GetFilteredAsync(bool? status, string searchTerm, List<int> tagIds, int page, int pageSize)
        {
            var query = _context.Notes
                .Include(n => n.Tags)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Title.Contains(searchTerm) || p.Content.Contains(searchTerm));
            }

            if (tagIds != null && tagIds.Any())
            {
                query = query.Where(n => tagIds.All(id => n.Tags.Any(t => t.Id == id)));
            }

            int totalCount = await query.CountAsync();

            var notes = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (notes, totalCount);
        }

        public async Task<Note> AddAsync(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task UpdateAsync(Note note)
        {
            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int id, bool status)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                note.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Notes.AnyAsync(e => e.Id == id);
        }
    }
}
