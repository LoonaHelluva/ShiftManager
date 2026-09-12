using System;

namespace HelipadManager;

public interface ITaskDbService
{
    //Create
    Task<HeliTask> AddTaskAsync(AddTaskDto task);

    //Read
    Task<HeliTask> GetTaskByIdAsync(int id);
    Task<List<HeliTask>> GetTasks();

    //Update
    Task UpdateTaskByIdAsync(int id, UpdateTaskDto updatedTask);

    //Delete
    Task DeleteTaskById(int id);
}
