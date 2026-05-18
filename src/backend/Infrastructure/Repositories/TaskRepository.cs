using Microsoft.EntityFrameworkCore;
using TaskTrackerApi.Application.Interfaces;
using TaskTrackerApi.Domain.Entities;
using TaskTrackerApi.Infrastructure.Data;

namespace TaskTrackerApi.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;

    public TaskRepository(AppDbContext db) => _db = db;

    public async Task<List<TaskItem>> GetAllAsync(string? status, string? priority)
    {
        var query = _db.Tasks.AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(t => t.Status == status);

        if (!string.IsNullOrEmpty(priority))
            query = query.Where(t => t.Priority == priority);

        return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
        => await _db.Tasks.FindAsync(id);

    public async Task<List<TaskItem>> SearchAsync(string query)
    {
        var pattern = $"%{query}%";
        return await _db.Tasks
            .Where(t => EF.Functions.Like(t.Title, pattern) ||
                         (t.Description != null && EF.Functions.Like(t.Description, pattern)))
            .ToListAsync();
    }

    public async Task<TaskItem> CreateAsync(TaskItem task)
    {
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return task;
    }

    public async Task UpdateAsync(TaskItem task)
        => await _db.SaveChangesAsync();

    public async Task DeleteAsync(TaskItem task)
    {
        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
    }

    public async Task BulkUpdateStatusAsync(Dictionary<int, string> updates)
    {
        var ids = updates.Keys.ToList();
        var tasks = await _db.Tasks.Where(t => ids.Contains(t.Id)).ToListAsync();

        foreach (var task in tasks)
        {
            if (updates.TryGetValue(task.Id, out var status))
                task.Status = status;
        }

        await _db.SaveChangesAsync();
    }

    public Task<List<TaskItem>> GetAllRawAsync()
        => _db.Tasks.ToListAsync();
}
