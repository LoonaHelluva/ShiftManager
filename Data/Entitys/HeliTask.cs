using System;
using System.ComponentModel.DataAnnotations;

namespace HelipadManager;

public class HeliTask
{
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty; //REQUIRED
    public string Desctiprion { get; set; } = String.Empty;
    public bool IsDone { get; set; } = false;

    public List<SubTask> SubTasks { get; set; } = new();
    public List<StaffMember> Executors { get; set; } = new();
    public int ShiftId { get; set; } //REQUIRED
    public int HeliId { get; set; } //REQUIRED

    private HeliTask()
    {
    }

    public HeliTask(string title, string description, int shiftId, int heliId, List<StaffMember> executor)
    {
        Title = title;
        Desctiprion = description;
        ShiftId = shiftId;
        HeliId = heliId;
        Executors = executor;
    }
}