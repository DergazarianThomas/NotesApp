using BackendNotes.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendNotes.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetByIdsAsync(List<int> tagIds);
        Task<IEnumerable<Tag>> GetAllTagsAsync(bool includeNotes = false);
        Task<Tag> GetTagByIdAsync(int id);
        Task<Tag> CreateTagAsync(Tag tag);
        Task<bool> DeleteTagAsync(int id);
        Task<bool> TagExistsAsync(int id);
        Task SaveChangesAsync();
    }

    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetByIdsAsync(List<int> tagIds)
        {
            return await _context.Tags
                .Where(t => tagIds.Contains(t.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync(bool includeNotes = false)
        {
            IQueryable<Tag> query = _context.Tags;

            if (includeNotes)
            {
                query = query.Include(t => t.Notes);
            }

            return await query.ToListAsync();
        }

        public async Task<Tag> GetTagByIdAsync(int id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public async Task<Tag> CreateTagAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await SaveChangesAsync();
            return tag;
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            var tag = await GetTagByIdAsync(id);
            if (tag == null)
            {
                return false;
            }

            _context.Tags.Remove(tag);
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> TagExistsAsync(int id)
        {
            return await _context.Tags.AnyAsync(e => e.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

