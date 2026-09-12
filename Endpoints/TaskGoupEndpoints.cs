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
        tasks.MapPost("/", async (AddTaskDto taskToAdd, ITaskDbService dbService) =>
        {
            try
            {
                HeliTask task = await dbService.AddTaskAsync(taskToAdd);

                return Results.Created(projectUri + task.Id, task);
            }
            catch (KeyNotFoundException e)
            {
                return Results.NotFound(e.Message);
            }
            catch (Exception e)
            {
                return Results.InternalServerError(e.Message);
            }
        }).
        AddEndpointFilter(async (context, next) =>
        {
            AddTaskDto taskCheck = context.GetArgument<AddTaskDto>(0);

            if (taskCheck.HeliId < 0)
            {
                return Results.BadRequest("Helicopter id is less than 0");
            }
            if (taskCheck.ShiftId < 0)
            {
                return Results.BadRequest("Shift id is less than 0");
            }
            if (taskCheck.Executor.Count == 0)
            {
                return Results.BadRequest("Executors ammount less than 1");
            }

            return await next(context);
        });

        //PUT
        tasks.MapPut("/{id}", async (int id, UpdateTaskDto updatedTask, ITaskDbService dbService) =>
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
            UpdateTaskDto taskCheck = context.GetArgument<UpdateTaskDto>(1);

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
