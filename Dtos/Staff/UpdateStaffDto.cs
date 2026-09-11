namespace HelipadManager;

public record UpdateStaffDto(
    int Id,
    string? Name = null,
    int? ArmyNumber = null,
    bool? IsManager = null
);