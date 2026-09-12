namespace HelipadManager;

public record TaskDto(
    int Id,
    string Title,
    string Description,
    bool IsDone,
    List<StaffMember> Executors,
    int ShiftId,
    int HeliId
);