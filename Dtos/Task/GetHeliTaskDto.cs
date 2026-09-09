namespace HelipadManager;

public record GetHeliTaskDto(
    int Id,
    string Title,
    string Description,
    bool IsDone,
    List<SubTask> SubTasks,
    List<StaffMember> Executors,
    int ShiftId,
    int HeliId
);
