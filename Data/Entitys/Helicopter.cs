using System;
using System.ComponentModel.DataAnnotations;

namespace HelipadManager;

public class Helicopter
{
    public int Id { get; set; }
    public int TailNum { get; set; }
    public string Usability { get; set; } = null!; //REQUIRED
    public string FlightStatus { get; set; } = null!; //REQUIRED

    public List<HeliTask> Tasks { get; set; } = new();
    public List<Shift> Shifts { get; set; } = new();

    private Helicopter()
    {
    }

    public Helicopter(string usability, string flightStatus, int tailNum, Shift? shift = null)
    {
        Usability = usability;
        FlightStatus = flightStatus;
        TailNum = tailNum;

        if (shift != null)
        {
            Shifts.Add(shift);
        }
    }
}