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
                List<GetHeliDto> helis = await dbService.GetHelisAsync();

                return Results.Ok(helis);
            }
            catch (Exception e)
            {
                return Results.NotFound(e.Message);
            }
        });

        //POST
        helis.MapPost("/", async (AddHeliDto heli, IHelicopterDbService dbService) =>
        {
            var newHeli = await dbService.AddHeliAsync(heli);

            return Results.Accepted(projectUri + newHeli.Id, newHeli);
        })
        .AddEndpointFilter(async (context, next) =>
        {
            var heliCheck = context.GetArgument<AddHeliDto>(0);

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

            return await next(context);
        });

        //PUT
        helis.MapPut("/{id}", async (int id, UpdateHeliDto updatedHeli, IHelicopterDbService dbService) =>
        {
            await dbService.UpdateHeliByIdAsync(id, updatedHeli);

            return Results.Ok();
        })
        .AddEndpointFilter(async (context, next) =>
        {
            int idCheck = context.GetArgument<int>(0);
            var heliCheck = context.GetArgument<UpdateHeliDto>(1);

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
