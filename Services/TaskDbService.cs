using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HelipadManager;

public class TaskDbService : ITaskDbService
{
    private ShiftDbContext _db;
    public TaskDbService(ShiftDbContext db)
    {
        _db = db;
    }


    //Add task to DB
    public async Task<HeliTask> AddTaskAsync(AddTaskDto task)
    {
        //Getting is helicopter and shift exists as bool
        bool isHeliExists = await _db.Helicopters.AnyAsync(h => h.Id == task.HeliId);
        bool isShiftExists = await _db.Shifts.AnyAsync(s => s.Id == task.ShiftId);

        //Checking helicopter shift  for existance
        if (isHeliExists == false)
        {
            throw new KeyNotFoundException($"Helicopter with id: {task.HeliId} was not found");
        }
        if (isShiftExists == false)
        {
            throw new KeyNotFoundException($"Shift with id: {task.ShiftId} was not found");
        }

        //Check of staff member
        List<StaffMember> staffs = new List<StaffMember>(); //New list of StaffMember to add to the task
        foreach (var id in task.Executor)
        {
            //Getting is staff exist
            StaffMember? staff = await _db.StaffMembers.FirstOrDefaultAsync(s => s.Id == id);

            //Checking the answer
            if (staff == null)
            {
                throw new KeyNotFoundException($"User with id: {id} was not found");
            }

            staffs.Add(staff); //Adding to list
        }

        //Creating new task
        HeliTask newTask = new HeliTask(
            task.Title,
            task.Description,
            task.ShiftId,
            task.HeliId,
            staffs
            );

        //Adding new task to table and saving changes
        await _db.Tasks.AddAsync(newTask);
        await _db.SaveChangesAsync();

        return newTask;
    }

    //Read
    public async Task<HeliTask> GetTaskByIdAsync(int id)
    {
        HeliTask? fromDbTask = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        if (fromDbTask == null)
        {
            throw new KeyNotFoundException();
        }

        return fromDbTask;
    }

    public async Task<List<HeliTask>> GetTasks()
    {
        List<HeliTask> tasks = await _db.Tasks.ToListAsync();

        if (tasks.Count == 0)
        {
            throw new KeyNotFoundException();
        }

        return tasks;
    }


    //Update
    public async Task UpdateTaskByIdAsync(int id, UpdateTaskDto updatedTask)
    {
        HeliTask? taskToUpdate = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        if (taskToUpdate == null)
        {
            throw new KeyNotFoundException();
        }

        if (updatedTask.Title != null)
        {
            taskToUpdate.Title = updatedTask.Title;
        }
        if (updatedTask.Description != null)
        {
            taskToUpdate.Desctiprion = updatedTask.Description;
        }
        if (updatedTask.IsDone != null)
        {
            taskToUpdate.IsDone = updatedTask.IsDone.Value;
        }

        await _db.SaveChangesAsync();
    }
    //Delete
    public async Task DeleteTaskById(int id)
    {
        await _db.Tasks.Where(t => t.Id == id).ExecuteDeleteAsync();
        await _db.SaveChangesAsync();
    }



}
