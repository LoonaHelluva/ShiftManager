using System;

namespace HelipadManager;

public interface ITaskDbService
{
    //Create
    Task<HeliTask> AddTaskAsync(AddHeliTaskDto task);

    //Read
    Task<HeliTask> GetTaskByIdAsync(int id);
    Task<List<HeliTask>> GetTasks();

    //Update
    Task UpdateTaskByIdAsync(int id, UpdateHeliTaskDto updatedTask);

    //Delete
    Task DeleteTaskById(int id);
}
