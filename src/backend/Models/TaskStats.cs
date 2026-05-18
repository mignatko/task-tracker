namespace TaskTrackerApi.Models;

public class TaskStats
{
    public int Total { get; set; }
    public int Todo { get; set; }
    public int InProgress { get; set; }
    public int Done { get; set; }
    public int Overdue { get; set; }
    public double AvgCompletionDays { get; set; }
}
