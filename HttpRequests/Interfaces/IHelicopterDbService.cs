using System;
using System.Threading.Tasks;

namespace HelipadManager;

public interface IHelicopterDbService
{
    //Create
    Task<Helicopter> AddHeliAsync(AddHelicopterDto helicopterDto);

    //Read
    Task<HelicopterDto> GetHeliByIdAsync(int id);
    Task<HelicopterDto> GetHeliWithListsByIdAsync(int id);
    Task<List<HelicopterDto>> GetHelisAsync();

    //Update
    Task UpdateHeliByIdAsync(int id, UpdateHelicopterDto updatedHeli);

    //Delete
    Task DeleteHeliByIdAsync(int id);
}