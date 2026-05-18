namespace TaskTrackerApi.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Priority { get; set; } = "Medium";
    public string Status { get; set; } = "Todo";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public string? AssignedTo { get; set; }
}

public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Priority { get; set; } = "Medium";
    public DateTime? DueDate { get; set; }
    public string? AssignedTo { get; set; }
}

public class UpdateTaskRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Priority { get; set; }
    public string? Status { get; set; }
    public DateTime? DueDate { get; set; }
    public string? AssignedTo { get; set; }
}
