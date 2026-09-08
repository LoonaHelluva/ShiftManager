using System;
using System.Threading.Tasks;

namespace HelipadManager;

public interface IHelicopterDbService
{
    //Create
    Task<Helicopter> AddHeliAsync(AddHeliDto helicopterDto);

    //Read
    Task<GetHeliDto> GetHeliByIdAsync(int id);
    Task<GetHeliDto> GetHeliWithListsByIdAsync(int id);
    Task<List<GetHeliDto>> GetHelisAsync();

    //Update
    Task UpdateHeliByIdAsync(int id, UpdateHeliDto updatedHeli);

    //Delete
    Task DeleteHeliByIdAsync(int id);
}