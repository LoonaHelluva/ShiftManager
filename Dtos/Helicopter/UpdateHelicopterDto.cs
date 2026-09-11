namespace HelipadManager;

public record UpdateHelicopterDto(
    int Id,
    int? TailNum = null,
    string? Usability = null,
    string? FlightStatus = null
);