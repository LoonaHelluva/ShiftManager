namespace HelipadManager;

public record UpdateStaffDto(
    string? Name = null,
    int? ArmyNumber = null,
    bool? IsManager = null
);