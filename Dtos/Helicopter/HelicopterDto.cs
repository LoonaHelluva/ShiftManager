namespace HelipadManager;

public record HelicopterDto(
    int Id,
    int TailNum,
    string Usability,
    string FlightStatus,
    List<HeliTask> Tasks,
    List<Shift> Shifts
);