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


    //Create
    public async Task<HeliTask> AddTaskAsync(AddTaskDto task)
    {
        HeliTask newTask = new HeliTask(
            task.Title,
            task.Description,
            task.ShiftId,
            task.HeliId,
            task.Executor
            );

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
