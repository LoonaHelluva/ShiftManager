using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
using SQLitePCL;

namespace HelipadManager;

public static class ShiftEndpoints
{
    public static void MapShiftEndpoints(this WebApplication app)
    {
        string projectUri = "http://localhost:5241/shifts/";

        var shifts = app.MapGroup("/shifts");

        //GET
        shifts.MapGet("/", async (IShiftDbService dbService) =>
        {
            try
            {
                List<ShiftDto> shifts = await dbService.GetShiftsAsync();

                return Results.Ok(shifts);
            }
            catch (KeyNotFoundException)
            {
                return Results.Ok(new List<ShiftDto>());
            }
            catch (Exception e)
            {
                return Results.InternalServerError(e.Message);
            }
        });

        shifts.MapGet("/{id}", async (int id, IShiftDbService dbService) =>
        {
            try
            {
                ShiftDto shift = await dbService.GetShiftByIdAsync(id);

                return Results.Ok(shift);
            }
            catch (KeyNotFoundException NotFound)
            {
                return Results.NotFound(NotFound.Message);
            }
            catch (Exception e)
            {
                return Results.InternalServerError(e.Message);
            }
        }).
        AddEndpointFilter(async (context, next) =>
        {
            int idCheck = context.GetArgument<int>(0);

            if (idCheck < 0)
            {
                return Results.BadRequest("Incorrect id");
            }

            return await next(context);
        });

        //POSTers
        shifts.MapPost("/", async (AddShiftDto shift, IShiftDbService dbservice) =>
        {
            try
            {
                ShiftDto addedShift = await dbservice.AddShiftAsync(shift);

                return addedShift.Id == 0 ? Results.BadRequest(addedShift)
                                            : Results.Created(
                                                projectUri + addedShift.Id,
                                                addedShift
                                                );
            }
            catch (KeyNotFoundException e)
            {
                return Results.BadRequest(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return Results.BadRequest(e.Message);
            }
        }).
        AddEndpointFilter(async (context, next) =>
        {
            AddShiftDto shiftCheck = context.GetArgument<AddShiftDto>(0);

            if (shiftCheck == null)
            {
                return Results.BadRequest("Shift entity is null");
            }

            return await next(context);
        });

        //PUTers
        shifts.MapPut("/{id}", async (int id, UpdateShiftDto shiftToUpdate, IShiftDbService dbService) =>
        {
            try
            {
                await dbService.UpdateShiftById(id, shiftToUpdate);

                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.InternalServerError(e.Message);
            }
        }).
        AddEndpointFilter(async (context, next) =>
        {
            int idCheck = context.GetArgument<int>(0);
            UpdateShiftDto shiftCheck = context.GetArgument<UpdateShiftDto>(1);

            if (shiftCheck == null)
            {
                return Results.BadRequest("Shift entity is incorrect");
            }
            if (idCheck < 0)
            {
                return Results.BadRequest("Incorrect shift id");
            }
            if (!shiftCheck.Date.HasValue && !shiftCheck.ManagerId.HasValue)
            {
                return Results.BadRequest("Update shift values are empty");
            }
            if (shiftCheck.ManagerId.HasValue && shiftCheck.ManagerId.Value < 0)
            {
                return Results.BadRequest("Manager Id is less than 0");
            }

            return await next(context);
        });

        //DELETEers
        shifts.MapDelete("/{id}", async (int id, IShiftDbService dbService) =>
        {
            try
            {
                await dbService.DeleteShiftById(id);

                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.InternalServerError(e.Message);
            }
        }).
        AddEndpointFilter(async (context, next) =>
        {
            int idCheck = context.GetArgument<int>(0);

            if (idCheck < 0)
            {
                return Results.BadRequest("Id is less than 0");
            }

            return await next(context);
        });
    }
}