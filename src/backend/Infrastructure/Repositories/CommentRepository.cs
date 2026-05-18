using Microsoft.EntityFrameworkCore;
using TaskTrackerApi.Application.Interfaces;
using TaskTrackerApi.Domain.Entities;
using TaskTrackerApi.Infrastructure.Data;

namespace TaskTrackerApi.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _db;

    public CommentRepository(AppDbContext db) => _db = db;

    public async Task<List<Comment>> GetByTaskIdAsync(int taskId)
        => await _db.Comments
            .Where(c => c.TaskItemId == taskId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

    public async Task<Comment?> GetByIdAsync(int commentId)
        => await _db.Comments.FindAsync(commentId);

    public async Task<Comment> CreateAsync(Comment comment)
    {
        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();
        return comment;
    }

    public async Task DeleteAsync(Comment comment)
    {
        _db.Comments.Remove(comment);
        await _db.SaveChangesAsync();
    }
}
