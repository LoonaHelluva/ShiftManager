namespace HelipadManager;

public record AddStaffDto(
    string Name,
    int ArmyNumber,
    bool IsManager
);