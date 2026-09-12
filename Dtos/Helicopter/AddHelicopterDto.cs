namespace HelipadManager;

public record AddHelicopterDto(
    int TailNum,
    string Usability = "Ok",
    string FlightStatus = "Grounded",
    int? ShiftId = null
);