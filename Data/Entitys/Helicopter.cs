using System;
using System.ComponentModel.DataAnnotations;

namespace HelipadManager;

public class Helicopter
{
    public int Id { get; set; }
    public int TailNum { get; set; }
    public string Usability { get; set; } //REQUIRED
    public string FlightStatus { get; set; } //REQUIRED

    public List<Task> Tasks { get; set; } = new();
    public List<Shift> Shifts { get; set; } = new();

    public Helicopter(string usability, string flightStatus, int tailNum)
    {
        Usability = usability;
        FlightStatus = flightStatus;
        TailNum = tailNum;
    }
}