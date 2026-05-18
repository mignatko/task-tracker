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

app.Run();
