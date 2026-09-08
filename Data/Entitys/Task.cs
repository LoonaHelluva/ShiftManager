using System;
using System.ComponentModel.DataAnnotations;

namespace HelipadManager;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; } //REQUIRED
    public string Desctiprion { get; set; } = String.Empty;
    public bool IsDone { get; set; } = false;

    public List<SubTask> SubTasks { get; set; } = new();
    public List<StaffMember> Executors { get; set; } = new();
    public int ShiftId { get; set; } //REQUIRED
    public int HeliId { get; set; } //REQUIRED

    public Task(string title, int shiftId, int heliId)
    {
        Title = title;
        ShiftId = shiftId;
        HeliId = heliId;
    }
}