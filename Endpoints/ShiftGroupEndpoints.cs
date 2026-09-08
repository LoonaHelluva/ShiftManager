using System;
using System.Runtime.CompilerServices;

namespace HelipadManager;

public static class ShiftEndpoints
{
    public static void MapShiftEndpoints(this WebApplication app)
    {
        string projectUri = "http://localhost:5241/shifts/";

        var shifts = app.MapGroup("/shifts");

        //GETters
        shifts.MapGet("/", async (IShiftDbService dbService) =>
        {
            List<Shift> shifts = await dbService.GetShiftsAsync();

            if (shifts.Count == 0)
            {
                return Results.NotFound();
            }

            return Results.Ok(shifts);
        });

        shifts.MapGet("/{id}", async (int id, IShiftDbService dbService) =>
        {
            var shift = await dbService.GetShiftByIdAsync(id);

            if (shift == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(shift);
        });

        //POSTers
        shifts.MapPost("/", async (AddShiftDto shift, IShiftDbService dbservice) =>
        {
            Shift addedShift = await dbservice.AddShiftAsync(shift);

            return addedShift.Id == 0 ? Results.BadRequest(addedShift)
                                        : Results.Created(
                                            projectUri + addedShift.Id,
                                            addedShift
                                            );
        });

        //PUTers

        //DELETEers
    }
}