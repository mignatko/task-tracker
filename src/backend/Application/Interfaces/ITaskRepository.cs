using TaskTrackerApi.Domain.Entities;

namespace TaskTrackerApi.Application.Interfaces;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllAsync(string? status, string? priority);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<List<TaskItem>> SearchAsync(string query);
    Task<TaskItem> CreateAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
    Task BulkUpdateStatusAsync(Dictionary<int, string> updates);
    Task<List<TaskItem>> GetAllRawAsync();
}
