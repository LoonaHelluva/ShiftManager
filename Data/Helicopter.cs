using System;
using System.ComponentModel.DataAnnotations;

namespace HelipadManager;

public class Helicopter
{
    public int Id { get; set; }
    public string UsabilityStatus { get; set; } //REQUIRED
    public string FlightStatus { get; set; } //REQUIRED

    public List<Task> Tasks { get; set; } = new();
    public List<Shift> Shifts { get; set; } = new();

    public Helicopter(string usabilityStatus, string flightStatus)
    {
        UsabilityStatus = usabilityStatus;
        FlightStatus = flightStatus;
    }
}