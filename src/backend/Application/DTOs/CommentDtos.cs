using System.ComponentModel.DataAnnotations;

namespace TaskTrackerApi.Application.DTOs;

public record CreateCommentRequest(
    [Required, StringLength(100, MinimumLength = 1)] string Author,
    [Required, StringLength(5000, MinimumLength = 1)] string Content
);
