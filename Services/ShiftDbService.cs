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
    }

    //Create
    public async Task<ShiftDto> AddShiftAsync(AddShiftDto shift)
    {
        //Check on manager existance and if exist checking if manager
        StaffMember? staff = await _db.StaffMembers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == shift.ManagerId);
        if (staff == null)
        {
            throw new KeyNotFoundException($"Staff member with id: {shift.ManagerId} was not found");
        }
        if (staff.IsManager == false)
        {
            throw new InvalidOperationException($"Staff member with id: {staff.Id} is not manager");
        }
        Shift newShift = new Shift(shift.Date,
                                   shift.ManagerId
                                   );

        await _db.Shifts.AddAsync(newShift);
        await _db.SaveChangesAsync();

        ShiftDto shiftToSend = new ShiftDto(newShift.Id,
                                            newShift.Date,
                                            newShift.ManagerId
                                            );

        return shiftToSend;
    }

    //Reade
    public async Task<ShiftDto> GetShiftByIdAsync(int id)
    {
        Shift? shift = await _db.Shifts.FirstOrDefaultAsync(s => s.Id == id);

        if (shift == null)
        {
            throw new KeyNotFoundException($"There is no Shift with id:{id}");
        }

        ShiftDto shiftToSend = new ShiftDto(shift.Id, shift.Date, shift.ManagerId);
        return shiftToSend;
    }

    public async Task<List<ShiftDto>> GetShiftsAsync()
    {
        List<Shift> shifts = await _db.Shifts.AsNoTracking().ToListAsync();

        if (shifts.Count == 0)
        {
            throw new KeyNotFoundException($"Shifts table is empty, you need to create shift");
        }

        List<ShiftDto> shiftDtos = new List<ShiftDto>();

        foreach (var s in shifts)
        {
            shiftDtos.Add(new ShiftDto(s.Id, s.Date, s.ManagerId));
        }

        return shiftDtos;
    }

    //Update
    public async Task UpdateShiftById(int id, UpdateShiftDto updatedShift)
    {
        if (updatedShift == null)
        {
            throw new Exception($"The update entity can not be null");
        }

        Shift? shift = await _db.Shifts.FirstOrDefaultAsync(s => s.Id == id);

        if (shift == null)
        {
            throw new KeyNotFoundException($"Shift with id:{id} was not found");
        }

        if (updatedShift.Date.HasValue)
        {
            shift.Date = updatedShift.Date.Value;
        }
        if (updatedShift.ManagerId.HasValue)
        {
            shift.ManagerId = updatedShift.ManagerId.Value;
        }

        await _db.SaveChangesAsync();
    }

    //Delete
    public async Task DeleteShiftById(int id)
    {
        try
        {
            await _db.Shifts.Where(s => s.Id == id).ExecuteDeleteAsync();
        }
        catch { }
    }
}