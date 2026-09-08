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
    Task<int> AddTaskAsync(HeliTask task);
    Task<int> AddTaskWithSubTaskAsync(HeliTask task, SubTask subTask);
    Task<int> AddSubTaskByIdAsync(SubTask subTask, int connectedTaskId);

    //Reader
    //Staff

    //Shift
    Task<List<Shift>> GetShiftsAsync();
    Task<Shift> GetShiftByIdAsync(int id);
    //Updaters

    //Deleters
}
