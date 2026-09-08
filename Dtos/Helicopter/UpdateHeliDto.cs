namespace HelipadManager;

public record class UpdateHeliDto(
	int TailNum = -1,
	string? Usability = null,
	string? FlightStatus = null);
