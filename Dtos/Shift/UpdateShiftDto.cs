namespace HelipadManager;

public record UpdateShiftDto(
    DateOnly? Date = null,
    int? ManagerId = null
);