namespace TaskTrackerApi.Application.DTOs;

public record TaskStats(
    int Total,
    int Todo,
    int InProgress,
    int Done,
    int Overdue,
    double AvgCompletionDays
);
