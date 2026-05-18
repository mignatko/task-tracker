namespace TaskTrackerApi.Models;

public class Comment
{
    public int Id { get; set; }
    public int TaskItemId { get; set; }
    public string Author { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public TaskItem? TaskItem { get; set; }
}

public class CreateCommentRequest
{
    public string Author { get; set; } = "";
    public string Content { get; set; } = "";
}
