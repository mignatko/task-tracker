using TaskTrackerApi.Application.DTOs;
using TaskTrackerApi.Application.Services;

namespace TaskTrackerApi.Endpoints;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/tasks/{taskId:int}/comments");

        group.MapGet("/", async (int taskId, CommentService svc) =>
        {
            var comments = await svc.GetByTaskIdAsync(taskId);
            return Results.Ok(comments);
        });

        group.MapPost("/", async (int taskId, CreateCommentRequest request, CommentService svc) =>
        {
            var comment = await svc.CreateAsync(taskId, request);
            if (comment is null) return Results.NotFound(new { error = "Task not found" });
            return Results.Created($"/api/tasks/{taskId}/comments/{comment.Id}", comment);
        });

        group.MapDelete("/{commentId:int}", async (int taskId, int commentId, CommentService svc) =>
        {
            return await svc.DeleteAsync(taskId, commentId) ? Results.NoContent() : Results.NotFound();
        });
    }
}
