using System;

namespace HelipadManager;

public static class HelicopterEndpoints
{
    public static void MapHelicopterGroupEndpoints(this WebApplication app)
    {
        string projectUri = "http://localhost:5241/helis/";

        var helis = app.MapGroup("/helis/");

        //GET   
        helis.MapGet("/", async (IHelicopterDbService dbService) =>
        {
            try
            {
                List<HelicopterDto> helis = await dbService.GetHelisAsync();

                return Results.Ok(helis);
            }
            catch (Exception e)
            {
                return Results.NotFound(e.Message);
            }
        });

        //POST
        helis.MapPost("/", async (AddHelicopterDto heli, IHelicopterDbService dbService) =>
        {
            try
            {
                var newHeli = await dbService.AddHeliAsync(heli);

                return Results.Accepted(projectUri + newHeli.Id, newHeli);
            }
            catch (KeyNotFoundException e)
            {
                return Results.NotFound(e.Message);
            }
        })
        .AddEndpointFilter(async (context, next) =>
        {
            var heliCheck = context.GetArgument<AddHelicopterDto>(0);

            if (heliCheck.TailNum <= 0)
            {
                return Results.BadRequest();
            }
            if (heliCheck.Usability == string.Empty && heliCheck.Usability == " ")
            {
                return Results.BadRequest();
            }
            if (heliCheck.FlightStatus == string.Empty && heliCheck.FlightStatus == " ")
            {
                return Results.BadRequest();
            }
            if (heliCheck.ShiftId < 0)
            {
                return Results.BadRequest("Shift Id is less than 0");
            }

            return await next(context);
        });

        //PUT
        helis.MapPut("/{id}", async (int id, UpdateHelicopterDto updatedHeli, IHelicopterDbService dbService) =>
        {
            await dbService.UpdateHeliByIdAsync(id, updatedHeli);

            return Results.Ok();
        })
        .AddEndpointFilter(async (context, next) =>
        {
            int idCheck = context.GetArgument<int>(0);
            var heliCheck = context.GetArgument<UpdateHelicopterDto>(1);

            if (heliCheck == null || idCheck < 0)
            {
                return Results.BadRequest();
            }

            if (heliCheck.TailNum == -1)
            {
                if (heliCheck.Usability == null)
                {
                    if (heliCheck.FlightStatus == null)
                    {
                        return Results.BadRequest();
                    }
                }
            }

            return await next(context);

        });

        //DELETE
        helis.MapDelete("/{id}", async (int id, IHelicopterDbService dbService) =>
        {
            await dbService.DeleteHeliByIdAsync(id);

            return Results.NoContent();
        }).AddEndpointFilter(async (context, next) =>
        {
            int idCheck = context.GetArgument<int>(0);

            if (idCheck < 0)
            {
                return Results.BadRequest();
            }

            return await next(context);
        });
    }
}
