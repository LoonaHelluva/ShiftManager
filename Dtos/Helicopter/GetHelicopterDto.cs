namespace HelipadManager;

public record GetHeliDto(
    int Id,
    int TailNum,
    string Usability,
    string FlightStatus,
    List<Task> Tasks,
    List<Shift> Shifts
);