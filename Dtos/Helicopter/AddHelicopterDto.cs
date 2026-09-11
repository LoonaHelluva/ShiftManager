namespace HelipadManager;

public record AddHelicopterDto(
    int TailNum,
    string Usability,
    string FlightStatus
);