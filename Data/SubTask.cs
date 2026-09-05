using System;
using System.ComponentModel.DataAnnotations;

namespace HelipadManager;

public class SubTask
{
    public int Id { get; set; }
    [Required] public string Title { get; set; } //REQUIRED
    public string Desctiprion = String.Empty;
    public bool IsDone = false;

    [Required] public int TaskId { get; set; } //REQUIRED
    public List<StaffMember> Executors { get; set; } = new();

    public SubTask(string title, int taskId)
    {
        Title = title;
        TaskId = taskId;
    }
}