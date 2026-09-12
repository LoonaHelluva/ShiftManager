using System;

namespace HelipadManager;

public interface IShiftDbService
{
    //Creat
    Task<ShiftDto> AddShiftAsync(AddShiftDto shift);

    //Read
    Task<List<ShiftDto>> GetShiftsAsync();
    Task<ShiftDto> GetShiftByIdAsync(int id);

    //Update
    Task UpdateShiftById(int id, UpdateShiftDto updatedShift);

    //Delete
    Task DeleteShiftById(int id);
}
