using TaskTrackerApi.Domain.Entities;

namespace TaskTrackerApi.Application.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetByTaskIdAsync(int taskId);
    Task<Comment?> GetByIdAsync(int commentId);
    Task<Comment> CreateAsync(Comment comment);
    Task DeleteAsync(Comment comment);
}
