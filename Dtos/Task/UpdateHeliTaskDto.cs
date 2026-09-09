namespace HelipadManager;

public record class UpdateHeliTaskDto(
    string? Title = null,
    string? Description = null,
    bool? IsDone = null
);
