using Microsoft.EntityFrameworkCore;
using TaskTrackerApi.Data;
using TaskTrackerApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db"));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

app.MapGet("/api/tasks", async (AppDbContext db, string? status, string? priority) =>
{
    var query = db.Tasks.AsQueryable();

    if (!string.IsNullOrEmpty(status))
        query = query.Where(t => t.Status == status);

    if (!string.IsNullOrEmpty(priority))
        query = query.Where(t => t.Priority == priority);

    return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
});

app.MapGet("/api/tasks/{id}", async (int id, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    return task is not null ? Results.Ok(task) : Results.NotFound();
});

app.MapPost("/api/tasks", async (CreateTaskRequest request, AppDbContext db) =>
{
    var task = new TaskItem
    {
        Title = request.Title,
        Description = request.Description,
        Priority = request.Priority,
        DueDate = request.DueDate,
        AssignedTo = request.AssignedTo
    };

    db.Tasks.Add(task);
    await db.SaveChangesAsync();
    return Results.Created($"/api/tasks/{task.Id}", task);
});

app.MapPut("/api/tasks/{id}", async (int id, UpdateTaskRequest request, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    if (request.Title is not null) task.Title = request.Title;
    if (request.Description is not null) task.Description = request.Description;
    if (request.Priority is not null) task.Priority = request.Priority;
    if (request.Status is not null) task.Status = request.Status;
    if (request.DueDate is not null) task.DueDate = request.DueDate;
    if (request.AssignedTo is not null) task.AssignedTo = request.AssignedTo;

    await db.SaveChangesAsync();
    return Results.Ok(task);
});

app.MapDelete("/api/tasks/{id}", async (int id, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    db.Tasks.Remove(task);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Comments endpoints
app.MapGet("/api/tasks/{taskId}/comments", async (int taskId, AppDbContext db) =>
{
    var comments = await db.Comments
        .Where(c => c.TaskItemId == taskId)
        .OrderByDescending(c => c.CreatedAt)
        .ToListAsync();
    return Results.Ok(comments);
});

app.MapPost("/api/tasks/{taskId}/comments", async (int taskId, CreateCommentRequest request, AppDbContext db) =>
{
    var comment = new Comment
    {
        TaskItemId = taskId,
        Author = request.Author,
        Content = request.Content,
        CreatedAt = DateTime.Now
    };

    db.Comments.Add(comment);
    await db.SaveChangesAsync();
    return Results.Created($"/api/tasks/{taskId}/comments/{comment.Id}", comment);
});

app.MapDelete("/api/tasks/{taskId}/comments/{commentId}", async (int taskId, int commentId, AppDbContext db) =>
{
    var comment = await db.Comments.FindAsync(commentId);
    if (comment is null) return Results.NotFound();

    db.Comments.Remove(comment);
    await db.SaveChangesAsync();
    return Results.Ok(comment);
});

app.MapGet("/api/tasks/search", async (string q, AppDbContext db) =>
{
    var pattern = $"%{q}%";
    var tasks = await db.Tasks
        .Where(t => EF.Functions.Like(t.Title, pattern) ||
                     (t.Description != null && EF.Functions.Like(t.Description, pattern)))
        .ToListAsync();
    return Results.Ok(tasks);
});

// Stats endpoint
app.MapGet("/api/stats", async (AppDbContext db) =>
{
    var tasks = await db.Tasks.ToListAsync();
    var stats = new TaskStats
    {
        Total = tasks.Count,
        Todo = tasks.Count(t => t.Status == "Todo"),
        InProgress = tasks.Count(t => t.Status == "InProgress"),
        Done = tasks.Count(t => t.Status == "Done"),
        Overdue = tasks.Count(t => t.DueDate < DateTime.Now && t.Status != "Done"),
        AvgCompletionDays = tasks.Where(t => t.Status == "Done").Any()
            ? tasks.Where(t => t.Status == "Done").Average(t => (DateTime.Now - t.CreatedAt).TotalDays)
            : 0
    };
    return Results.Ok(stats);
});

// Bulk update endpoint
app.MapPost("/api/tasks/bulk-update", async (HttpContext context, AppDbContext db) =>
{
    var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
    var updates = System.Text.Json.JsonSerializer.Deserialize<List<BulkUpdateItem>>(body);

    if (updates is null)
        return Results.BadRequest("Invalid request body");

    foreach (var update in updates)
    {
        var task = await db.Tasks.FindAsync(update.Id);
        if (task is null) continue;
        task.Status = update.Status;
    }
    await db.SaveChangesAsync();
    return Results.Ok("Updated");
});

// Export tasks as CSV
app.MapGet("/api/tasks/export", async (AppDbContext db) =>
{
    var tasks = await db.Tasks.ToListAsync();
    var csv = "Id,Title,Description,Priority,Status,CreatedAt,DueDate,AssignedTo\n";
    foreach (var t in tasks)
    {
        csv += $"{t.Id},{t.Title},{t.Description},{t.Priority},{t.Status},{t.CreatedAt},{t.DueDate},{t.AssignedTo}\n";
    }
    return Results.Text(csv, "text/csv");
});

app.Run();

record BulkUpdateItem(int Id, string Status);
