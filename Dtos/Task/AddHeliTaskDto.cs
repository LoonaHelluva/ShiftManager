namespace HelipadManager;

public record AddHeliTaskDto(
    string Title,
    string Description,
    List<StaffMember> Executor,
    int HeliId,
    int ShiftId
);
