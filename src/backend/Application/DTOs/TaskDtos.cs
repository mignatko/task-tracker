using System.ComponentModel.DataAnnotations;

namespace TaskTrackerApi.Application.DTOs;

public record CreateTaskRequest(
    [Required, StringLength(200, MinimumLength = 1)] string Title,
    [StringLength(2000)] string? Description,
    [Required, RegularExpression("^(Low|Medium|High|Critical)$")] string Priority = "Medium",
    DateTime? DueDate = null,
    [StringLength(100)] string? AssignedTo = null
);

public record UpdateTaskRequest(
    [StringLength(200, MinimumLength = 1)] string? Title,
    [StringLength(2000)] string? Description,
    [RegularExpression("^(Low|Medium|High|Critical)$")] string? Priority,
    [RegularExpression("^(Todo|InProgress|Done)$")] string? Status,
    DateTime? DueDate,
    [StringLength(100)] string? AssignedTo
);

public record BulkUpdateItem(
    [Required] int Id,
    [Required, RegularExpression("^(Todo|InProgress|Done)$")] string Status
);
