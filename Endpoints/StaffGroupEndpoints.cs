using System;

namespace HelipadManager;

public static class StaffGroupEndpoints
{
    public static void MapStaffEndpoints(this WebApplication app)
    {
        string projectUri = "http://localhost:5241/staff/";

        var staff = app.MapGroup("/staff/");

        //GET
        //Get list of staff requset (no need for endpooint filter)
        staff.MapGet("/", async (IStaffDbService dbService) =>
        {
            //Setting the try/catch bracket
            try
            {
                //Getting list of staff members isolated by dto
                List<StaffDto> staffs = await dbService.GetStaffsAsync();

                //Response as list to user
                return Results.Ok(staffs);
            }
            //Catching exception and sending to user
            catch (KeyNotFoundException)
            {
                return Results.NotFound("The table is empty, add values first");
            }
            catch (Exception e)
            {
                //Response as http error code with coment
                return Results.InternalServerError(e.Message);
            }
        });

        //Get staff member by id
        staff.MapGet("/{id}", async (int id, IStaffDbService dbService) =>
        {
            try
            {
                StaffDto staff = await dbService.GetStaffByIdAsync(id);

                return Results.Ok(staff);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound($"Staff member by id:{id} is not exist");
            }
        }).
        //Checking id throught endpoint filter
        AddEndpointFilter(async (context, next) =>
        {
            //Gettign id from context
            int idCheck = context.GetArgument<int>(0);

            //Id less than 0 check
            if (idCheck < 0)
            {
                return Results.BadRequest("The id is less than 0");
            }

            //Continuing to the body of endpoint if id is 0 or more
            return await next(context);
        });

        //POST
        //Add new staff memeber request with AddStaffDto
        staff.MapPost("/", async (AddStaffDto staffToAdd, IStaffDbService dbServide) =>
        {
            StaffDto staff = await dbServide.AddStaffAsync(staffToAdd);

            return Results.Created(projectUri + staff.Id, staff);
        })
        //Checking staff member from requset throught endpoint filter
        .AddEndpointFilter(async (context, next) =>
        {
            //Getting AddStaffDto from context by index 0
            AddStaffDto staffCehck = context.GetArgument<AddStaffDto>(0);

            //Checking if entity from requset is null
            if (staffCehck == null)
            {
                return Results.BadRequest("Entity is null");
            }

            //Continuing to the body of endpoint
            return await next(context);
        });

        //PUT
        //Update Staff member by id request with UpdateStaffDto
        staff.MapPut("/{id}", async (int id, UpdateStaffDto updatedStaff, IStaffDbService dbService) =>
        {
            await dbService.UpdateStaffByIdAsync(id, updatedStaff);

            return Results.Ok();
        })
        //Checking id and request entity with EndpointFilter
        .AddEndpointFilter(async (context, next) =>
        {
            //Getting entity from context
            int idCheck = context.GetArgument<int>(0);
            UpdateStaffDto staffCheck = context.GetArgument<UpdateStaffDto>(1);

            //Checking if entity is null
            if (staffCheck == null)
            {
                return Results.BadRequest("Update Entity is null");
            }

            //Checking if id is less than 0
            if (idCheck < 0)
            {
                return Results.BadRequest("Id is less than 0");
            }

            //Checking if all of Update Entity are null
            if ((staffCheck.Name == null || staffCheck.Name == string.Empty) &&
               staffCheck.ArmyNumber == null &&
               staffCheck.IsManager == null)
            {
                return Results.BadRequest("All values of Staff Update entity are null");
            }

            //Continuing to the body of endpoint
            return await next(context);
        });

        //DELETE
        //Deleting staff member by id
        staff.MapDelete("/{id}", async (int id, IStaffDbService dbService) =>
        {
            await dbService.DeleteStaffById(id);

            return Results.NoContent();
        })
        //Checking id throught endpoint diletr
        .AddEndpointFilter(async (context, next) =>
        {
            //Getting id from context
            int idCheck = context.GetArgument<int>(0);

            //Checking if id is less than 0
            if (idCheck < 0)
            {
                return Results.BadRequest("Id is less than 0");
            }

            //Continuing to the body of endpoint
            return await next(context);
        });
    }
}
