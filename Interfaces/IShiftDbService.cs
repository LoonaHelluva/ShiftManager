using System;

namespace HelipadManager;

public interface IShiftDbService
{
    //Creators
    //Staff
    Task<int> AddStaffMemberAsync(StaffMember staffMember);

    //Shift
    Task<Shift> AddShiftAsync(AddShiftDto shift);

    //Tasks
    Task<int> AddTaskAsync(Task task);
    Task<int> AddTaskWithSubTaskAsync(Task task, SubTask subTask);
    Task<int> AddSubTaskByIdAsync(SubTask subTask, int connectedTaskId);

    //Helicopters
    Task<int> AddHelicopterAsync(Helicopter helicopter);
    Task<int> AddHelicopterWithTasksAsync(Helicopter helicopter, List<Task> tasks);

    //Reader
    //Staff

    //Shift
    Task<List<Shift>> GetShiftsAsync();
    Task<Shift> GetShiftByIdAsync(int id);
    //Updaters
    void UpdateShiftByIdAsync(int id, UpdateShiftDto shift);

    //Deleters
}
