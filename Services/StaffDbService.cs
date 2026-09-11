using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace HelipadManager;

public class StaffDbService : IStaffDbService
{
    private ShiftDbContext _db;

    public StaffDbService(ShiftDbContext db)
    {
        _db = db;
    }

    //Create
    public async Task<StaffDto> AddStaffAsync(AddStaffDto staff)
    {
        //Parsing AddStaffDto => StaffMember
        StaffMember newStaff = new StaffMember(staff.Name,
                                                staff.ArmyNumber,
                                                staff.IsManager
                                              );

        try
        {
            //Adding value to table "StaffBMember" and saving changes
            await _db.StaffMembers.AddAsync(newStaff);
            await _db.SaveChangesAsync();
        }
        catch
        {
            //Throwing exception if catch on adding to table or saving changes in DB
            throw new Exception();
        }

        //Pasing Staffmember => StaffDto to return isolated entity to user
        StaffDto staffResult = new StaffDto(newStaff.Id,
                                                newStaff.Name,
                                                newStaff.ArmyNumber,
                                                newStaff.IsManager
                                            );

        return staffResult;
    }

    //Read
    public async Task<List<StaffDto>> GetStaffsAsync()
    {
        //Getting list of staff members and parsing to StaffDto
        List<StaffDto> stafs = await _db.StaffMembers.
                                            AsNoTracking().Select(staff => new StaffDto(staff.Id,
                                            staff.Name,
                                            staff.ArmyNumber,
                                            staff.IsManager)).
                                            ToListAsync();

        //Checking if we got the list
        if (stafs.Count == 0)
        {
            throw new KeyNotFoundException();
        }

        return stafs;
    }

    public async Task<StaffDto> GetStaffByIdAsync(int id)
    {
        //Getting staff member and parsing to StaffDto
        StaffDto? staff = await _db.StaffMembers.Where(s => s.Id == id).
                                                AsNoTracking().
                                                Select(staff => new StaffDto(staff.Id,
                                                                             staff.Name,
                                                                             staff.ArmyNumber,
                                                                             staff.IsManager)).
                                                FirstOrDefaultAsync();

        //Checking if got member
        if (staff == null)
        {
            throw new KeyNotFoundException();
        }

        //Returning member
        return staff;
    }

    //Update
    public async Task UpdateStaffByIdAsync(int id, UpdateStaffDto updatedStaff)
    {
        //Getting staff member
        StaffMember? staff = await _db.StaffMembers.FirstOrDefaultAsync(s => s.Id == id);

        //Checking if got the member
        if (staff == null)
        {
            throw new KeyNotFoundException();
        }

        //Updatign values if are exists
        //(Helps to udpate values seperatly while using one DTO)
        if (updatedStaff.IsManager.HasValue)
        {
            staff.IsManager = updatedStaff.IsManager.Value;
        }
        if (updatedStaff.ArmyNumber.HasValue)
        {
            staff.ArmyNumber = updatedStaff.ArmyNumber.Value;
        }
        if (updatedStaff.Name != null)
        {
            staff.Name = updatedStaff.Name;
        }

        //Saving changes
        await _db.SaveChangesAsync();
    }

    //Delete
    public async Task DeleteStaffById(int id)
    {
        //Deliting value from table by Id
        await _db.StaffMembers.Where(s => s.Id == id).ExecuteDeleteAsync();
    }
}
