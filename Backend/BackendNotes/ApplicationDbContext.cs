using Microsoft.EntityFrameworkCore;
using BackendNotes.Entities;

namespace BackendNotes
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationship
            modelBuilder.Entity<Note>()
                .HasMany(n => n.Tags)
                .WithMany(t => t.Notes)
                .UsingEntity(j => j.ToTable("NoteTags"));

            base.OnModelCreating(modelBuilder);
        }

    }
}
