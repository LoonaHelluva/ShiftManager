namespace HelipadManager;

public record UpdateHelicopterDto(
    int? TailNum = null,
    string? Usability = null,
    string? FlightStatus = null
);