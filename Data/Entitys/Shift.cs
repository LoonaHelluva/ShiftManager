using System;
using System.ComponentModel.DataAnnotations;

namespace HelipadManager;

public class Shift
{
    public int Id { get; set; }
    [Required] public DateOnly Date { get; set; }


    [Required] public int ManagerId { get; set; }
    public List<HeliTask> ManagerTasks { get; set; } = new();
    public List<Helicopter> Helicopters { get; set; } = new();
    public List<StaffMember> StaffMembers { get; set; } = new();

    public Shift(DateOnly date, int managerId)
    {
        Date = date;
        ManagerId = managerId;
    }
}