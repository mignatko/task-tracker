using TaskTrackerApi.Application.DTOs;
using TaskTrackerApi.Application.Services;

namespace TaskTrackerApi.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/tasks");

        group.MapGet("/", async (TaskService svc, string? status, string? priority) =>
        {
            var tasks = await svc.GetAllAsync(status, priority);
            return Results.Ok(tasks);
        });

        group.MapGet("/{id:int}", async (int id, TaskService svc) =>
        {
            var task = await svc.GetByIdAsync(id);
            return task is not null ? Results.Ok(task) : Results.NotFound();
        });

        group.MapPost("/", async (CreateTaskRequest request, TaskService svc) =>
        {
            var task = await svc.CreateAsync(request);
            return Results.Created($"/api/tasks/{task.Id}", task);
        });

        group.MapPut("/{id:int}", async (int id, UpdateTaskRequest request, TaskService svc) =>
        {
            var task = await svc.UpdateAsync(id, request);
            return task is not null ? Results.Ok(task) : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int id, TaskService svc) =>
        {
            return await svc.DeleteAsync(id) ? Results.NoContent() : Results.NotFound();
        });

        group.MapGet("/search", async (string q, TaskService svc) =>
        {
            var tasks = await svc.SearchAsync(q);
            return Results.Ok(tasks);
        });

        group.MapPost("/bulk-update", async (List<BulkUpdateItem> updates, TaskService svc) =>
        {
            var count = await svc.BulkUpdateAsync(updates);
            return Results.Ok(new { updated = count });
        });

        group.MapGet("/export", async (TaskService svc) =>
        {
            var csv = await svc.ExportCsvAsync();
            return Results.Text(csv, "text/csv");
        });
    }
}
