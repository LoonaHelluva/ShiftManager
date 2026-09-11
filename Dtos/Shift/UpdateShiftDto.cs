namespace HelipadManager;

public record UpdateShiftDto(
    int Id,
    DateOnly? Date = null,
    int? ManagerId = null
);