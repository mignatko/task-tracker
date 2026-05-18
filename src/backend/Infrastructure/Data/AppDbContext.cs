using Microsoft.EntityFrameworkCore;
using TaskTrackerApi.Domain.Entities;

namespace TaskTrackerApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(e =>
        {
            e.HasMany(t => t.Comments)
             .WithOne(c => c.TaskItem)
             .HasForeignKey(c => c.TaskItemId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Comment>(e =>
        {
            e.Property(c => c.Author).HasMaxLength(100);
            e.Property(c => c.Content).HasMaxLength(5000);
        });

        modelBuilder.Entity<TaskItem>(e =>
        {
            e.Property(t => t.Title).HasMaxLength(200);
            e.Property(t => t.Description).HasMaxLength(2000);
            e.Property(t => t.Priority).HasMaxLength(20);
            e.Property(t => t.Status).HasMaxLength(20);
            e.Property(t => t.AssignedTo).HasMaxLength(100);
        });
    }
}
