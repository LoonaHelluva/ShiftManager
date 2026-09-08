using System;

namespace HelipadManager;

public interface IHelicopterDbService
{
    //Create
    Task<int> AddHeliAsync(AddHeliDto helicopterDto);
    //Read
    Task<GetHeliDto?> GetHeliByIdAsync(int id);
    Task<GetHeliDto?> GetHeliWithListsByIdAsync(int id);
    Task<List<GetHeliDto?>?> GetHelisAsync();
    //Update

    //Delete
}