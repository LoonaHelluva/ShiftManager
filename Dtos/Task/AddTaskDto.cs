namespace HelipadManager;

public record AddTaskDto(
    string Title,
    string Description,
    List<StaffMember> Executor,
    int HeliId,
    int ShiftId
);