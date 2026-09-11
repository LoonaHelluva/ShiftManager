namespace HelipadManager;

public record UpdateTaskDto(
    int Id,
    string? Title = null,
    string? Description = null,
    bool? IsDone = null
);