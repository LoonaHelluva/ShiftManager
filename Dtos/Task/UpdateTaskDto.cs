namespace HelipadManager;

public record UpdateTaskDto(
    string? Title = null,
    string? Description = null,
    bool? IsDone = null
);