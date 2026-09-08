namespace HelipadManager;

public record GetHeliDto(
    int Id,
    int TailNum,
    string Usability,
    string FlightStatus,
    List<HeliTask> Tasks,
    List<Shift> Shifts
);