using System;
using System.ComponentModel.DataAnnotations;

namespace HelipadManager;

public class StaffMember
{
    public int Id { get; set; }
    [Required] public string Name { get; set; } = null!;
    [Required] public int ArmyNumber { get; set; }

    //Hide if not requested from Shift Table
    [Required] public bool IsManager { get; set; }
    public bool IsNightShift = false;

    public List<HeliTask> Tasks = new();
    public List<Shift> Shifts = new();

    private StaffMember()
    {
    }

    public StaffMember(string name, int armyNumber, bool isManager)
    {
        Name = name;
        ArmyNumber = armyNumber;
        IsManager = isManager;
    }
}