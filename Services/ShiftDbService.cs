using System;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

namespace HelipadManager;

public class ShiftDbService : IShiftDbService
{
    private ShiftDbContext _db;
    public ShiftDbService(ShiftDbContext db)
    {
        _db = db;
        _db.Database.EnsureCreated();
    }

    //Staff//
    //Create
    public async Task<int> AddStaffMemberAsync(StaffMember staffMember)
    {
        throw new NotImplementedException();
    }

    //Helicopter//
    //Create
    public async Task<int> AddHelicopterAsync(Helicopter helicopter)
    {
        throw new NotImplementedException();
    }

    public async Task<int> AddHelicopterWithTasksAsync(Helicopter helicopter, List<Task> tasks)
    {
        throw new NotImplementedException();
    }

    //Shift//
    //Create
    public async Task<Shift> AddShiftAsync(AddShiftDto shift)
    {
        Shift newShift = new Shift(
            shift.Date,
            shift.ManagerId
        );

        await _db.Shifts.AddAsync(newShift);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch
        {
            return newShift;
        }

        return newShift;
    }

    //Read
    public async Task<List<Shift>> GetShiftsAsync()
    {
        List<Shift> shifts = new List<Shift>();
        try
        {
            shifts = await _db.Shifts.ToListAsync<Shift>();
        }
        catch
        {
            return new List<Shift>();
        }

        return shifts;
    }

    public async Task<Shift> GetShiftByIdAsync(int id)
    {
        var shift = await _db.Shifts.FirstOrDefaultAsync(s => s.Id == id);

        if (shift == null)
        {
            return new Shift(DateOnly.FromDayNumber(00 - 00 - 0000), -1);
        }

        return shift;
    }

    //Update
    public async void UpdateShiftByIdAsync(int id, UpdateShiftDto shift)
    {
        var shiftToUpdate = await _db.Shifts.FirstOrDefaultAsync(s => s.Id == id);

        shiftToUpdate.ManagerId = shift.ManagerId;

        await _db.SaveChangesAsync();
    }

    //Task//
    //Create
    public async Task<int> AddSubTaskByIdAsync(SubTask subTask, int connectedTaskId)
    {
        throw new NotImplementedException();
    }

    public async Task<int> AddTaskAsync(Task task)
    {
        throw new NotImplementedException();
    }

    public async Task<int> AddTaskWithSubTaskAsync(Task task, SubTask subTask)
    {
        throw new NotImplementedException();
    }
}