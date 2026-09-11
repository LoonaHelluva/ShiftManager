using System;

namespace HelipadManager;

public interface IStaffDbService
{
    //Create
    Task<StaffDto> AddStaffAsync(AddStaffDto staff);

    //Read
    Task<List<StaffDto>> GetStaffsAsync();
    Task<StaffDto> GetStaffByIdAsync(int id);

    //Update
    Task UpdateStaffByIdAsync(int id, UpdateStaffDto updatedStaff);

    //Delete
    Task DeleteStaffById(int id);
}
