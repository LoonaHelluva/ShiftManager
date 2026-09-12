namespace HelipadManager;

public record AddTaskDto(
    string Title,
    string Description,
    List<int> Executor,
    int HeliId,
    int ShiftId
);