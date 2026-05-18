using System.Text;
using TaskTrackerApi.Application.DTOs;
using TaskTrackerApi.Application.Interfaces;
using TaskTrackerApi.Domain.Entities;

namespace TaskTrackerApi.Application.Services;

public class TaskService
{
    private readonly ITaskRepository _taskRepo;

    public TaskService(ITaskRepository taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public Task<List<TaskItem>> GetAllAsync(string? status, string? priority)
        => _taskRepo.GetAllAsync(status, priority);

    public Task<TaskItem?> GetByIdAsync(int id)
        => _taskRepo.GetByIdAsync(id);

    public Task<List<TaskItem>> SearchAsync(string query)
        => _taskRepo.SearchAsync(query);

    public async Task<TaskItem> CreateAsync(CreateTaskRequest request)
    {
        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            DueDate = request.DueDate,
            AssignedTo = request.AssignedTo
        };
        return await _taskRepo.CreateAsync(task);
    }

    public async Task<TaskItem?> UpdateAsync(int id, UpdateTaskRequest request)
    {
        var task = await _taskRepo.GetByIdAsync(id);
        if (task is null) return null;

        if (request.Title is not null) task.Title = request.Title;
        if (request.Description is not null) task.Description = request.Description;
        if (request.Priority is not null) task.Priority = request.Priority;
        if (request.Status is not null) task.Status = request.Status;
        if (request.DueDate is not null) task.DueDate = request.DueDate;
        if (request.AssignedTo is not null) task.AssignedTo = request.AssignedTo;

        await _taskRepo.UpdateAsync(task);
        return task;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _taskRepo.GetByIdAsync(id);
        if (task is null) return false;

        await _taskRepo.DeleteAsync(task);
        return true;
    }

    public async Task<int> BulkUpdateAsync(List<BulkUpdateItem> updates)
    {
        var dict = updates.ToDictionary(u => u.Id, u => u.Status);
        await _taskRepo.BulkUpdateStatusAsync(dict);
        return updates.Count;
    }

    public async Task<TaskStats> GetStatsAsync()
    {
        var tasks = await _taskRepo.GetAllRawAsync();
        var doneTasks = tasks.Where(t => t.Status == "Done").ToList();

        return new TaskStats(
            Total: tasks.Count,
            Todo: tasks.Count(t => t.Status == "Todo"),
            InProgress: tasks.Count(t => t.Status == "InProgress"),
            Done: doneTasks.Count,
            Overdue: tasks.Count(t => t.DueDate < DateTime.UtcNow && t.Status != "Done"),
            AvgCompletionDays: doneTasks.Count > 0
                ? doneTasks.Average(t => (DateTime.UtcNow - t.CreatedAt).TotalDays)
                : 0
        );
    }

    public async Task<string> ExportCsvAsync()
    {
        var tasks = await _taskRepo.GetAllRawAsync();
        var sb = new StringBuilder();
        sb.AppendLine("Id,Title,Description,Priority,Status,CreatedAt,DueDate,AssignedTo");

        foreach (var t in tasks)
        {
            sb.AppendLine(string.Join(",",
                CsvEscape(t.Id.ToString()),
                CsvEscape(t.Title),
                CsvEscape(t.Description ?? ""),
                CsvEscape(t.Priority),
                CsvEscape(t.Status),
                CsvEscape(t.CreatedAt.ToString("o")),
                CsvEscape(t.DueDate?.ToString("o") ?? ""),
                CsvEscape(t.AssignedTo ?? "")
            ));
        }

        return sb.ToString();
    }

    private static string CsvEscape(string value)
    {
        if (value.Contains('"') || value.Contains(',') || value.Contains('\n') || value.Contains('\r'))
            return $"\"{value.Replace("\"", "\"\"")}\"";
        return value;
    }
}
