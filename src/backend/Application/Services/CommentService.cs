using System.Net;
using TaskTrackerApi.Application.DTOs;
using TaskTrackerApi.Application.Interfaces;
using TaskTrackerApi.Domain.Entities;

namespace TaskTrackerApi.Application.Services;

public class CommentService
{
    private readonly ICommentRepository _commentRepo;
    private readonly ITaskRepository _taskRepo;

    public CommentService(ICommentRepository commentRepo, ITaskRepository taskRepo)
    {
        _commentRepo = commentRepo;
        _taskRepo = taskRepo;
    }

    public Task<List<Comment>> GetByTaskIdAsync(int taskId)
        => _commentRepo.GetByTaskIdAsync(taskId);

    public async Task<Comment?> CreateAsync(int taskId, CreateCommentRequest request)
    {
        var task = await _taskRepo.GetByIdAsync(taskId);
        if (task is null) return null;

        var comment = new Comment
        {
            TaskItemId = taskId,
            Author = WebUtility.HtmlEncode(request.Author),
            Content = WebUtility.HtmlEncode(request.Content)
        };

        return await _commentRepo.CreateAsync(comment);
    }

    public async Task<bool> DeleteAsync(int taskId, int commentId)
    {
        var comment = await _commentRepo.GetByIdAsync(commentId);
        if (comment is null || comment.TaskItemId != taskId) return false;

        await _commentRepo.DeleteAsync(comment);
        return true;
    }
}
