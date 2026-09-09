using System;
using Microsoft.AspNetCore.Http.HttpResults;

namespace HelipadManager;

public static class TaskGoupEndpoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        string projectUri = "http://localhost:5241/tasks/";

        var tasks = app.MapGroup("/tasks/");

        //GET
        tasks.MapGet("/", async (ITaskDbService dbService) =>
        {
            try
            {
                List<HeliTask> tasks = await dbService.GetTasks();

                return Results.Ok(tasks);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound("No Tasks in the table");
            }
        });

        tasks.MapGet("/{id}", async (int id, ITaskDbService dbService) =>
        {
            try
            {
                HeliTask task = await dbService.GetTaskByIdAsync(id);

                return Results.Ok(task);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound($"Task with id:{id}, was not found");
            }
        }).AddEndpointFilter(async (context, next) =>
        {
            int idCheck = context.GetArgument<int>(0);

            if (idCheck < 0)
            {
                return Results.BadRequest();
            }

            return next(context);
        });
        //POST
        tasks.MapPost("/", async (AddHeliTaskDto taskToAdd, ITaskDbService dbService) =>
        {
            HeliTask task = await dbService.AddTaskAsync(taskToAdd);

            return Results.Created(projectUri + task.Id, task);
        });

        //PUT
        tasks.MapPut("/{id}", async (int id, UpdateHeliTaskDto updatedTask, ITaskDbService dbService) =>
        {
            try
            {
                await dbService.UpdateTaskByIdAsync(id, updatedTask);

                return Results.Ok();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound($"Task with id:{id}, was not found");
            }
        }).AddEndpointFilter(async (context, next) =>
        {
            int idCheck = context.GetArgument<int>(0);
            UpdateHeliTaskDto taskCheck = context.GetArgument<UpdateHeliTaskDto>(1);

            if (idCheck < 0)
            {
                return Results.BadRequest("incorrect Id");
            }
            if (taskCheck.Title == null && taskCheck.Description == null && taskCheck.IsDone == null)
            {
                return Results.BadRequest("Updated task fields are empty");
            }

            return next(context);
        });

        //DELETE
        tasks.MapDelete("/{id}", async (int id, ITaskDbService dbService) =>
        {
            await dbService.DeleteTaskById(id);

            return Results.NoContent();
        }).AddEndpointFilter(async (context, next) =>
        {
            int idCheck = context.GetArgument<int>(0);

            if (idCheck < 0)
            {
                return Results.BadRequest("Id is less than 0");
            }

            return next(context);
        });
    }
}
