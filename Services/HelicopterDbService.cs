using System;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace HelipadManager;

public class HelicopterDbService : IHelicopterDbService
{
    private ShiftDbContext _db;
    public HelicopterDbService(ShiftDbContext db)
    {
        _db = db;
    }

    //Create
    public async Task<Helicopter> AddHeliAsync(AddHeliDto helicopterDto)
    {
        Helicopter newHelicopter = new(
                                        helicopterDto.Usability,
                                        helicopterDto.FlightStatus,
                                        helicopterDto.TailNum);

        await _db.Helicopters.AddAsync(newHelicopter);
        await _db.SaveChangesAsync();

        return newHelicopter;
    }

    //Read
    public async Task<GetHeliDto> GetHeliByIdAsync(int id)
    {
        var helicopter = await _db.Helicopters.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);

        if (helicopter is null)
        {
            throw new KeyNotFoundException($"Helicopter with id {id} was not found");
        }

        GetHeliDto returnHeli = new GetHeliDto(
                                                helicopter.Id,
                                                helicopter.TailNum,
                                                helicopter.Usability,
                                                helicopter.FlightStatus,
                                                new List<HeliTask>(),
                                                new List<Shift>()
        );

        return returnHeli;
    }
    public async Task<GetHeliDto> GetHeliWithListsByIdAsync(int id)
    {
        var helicopter = await _db.Helicopters.FirstOrDefaultAsync(h => h.Id == id);

        if (helicopter is null)
        {
            throw new KeyNotFoundException($"Helicopter with id {id} was not found");
        }

        GetHeliDto returnHeli = new GetHeliDto(
                                                helicopter.Id,
                                                helicopter.TailNum,
                                                helicopter.Usability,
                                                helicopter.FlightStatus,
                                                helicopter.Tasks,
                                                helicopter.Shifts
        );

        return returnHeli;
    }
    public async Task<List<GetHeliDto>> GetHelisAsync()
    {
        List<Helicopter>? helis = await _db.Helicopters.AsNoTracking().ToListAsync();

        if (helis is null)
        {
            throw new KeyNotFoundException($"Helicopter was not found");
        }

        List<GetHeliDto> returnHelis = new List<GetHeliDto>();

        foreach (var h in helis)
        {
            returnHelis.Add(new GetHeliDto(
                                                h.Id,
                                                h.TailNum,
                                                h.Usability,
                                                h.FlightStatus,
                                                h.Tasks,
                                                h.Shifts
            ));
        }

        return returnHelis;
    }

    //Update
    public async Task UpdateHeliByIdAsync(int id, UpdateHeliDto updatedHeli)
    {
        Helicopter? heliToUpdate = await _db.Helicopters.FirstOrDefaultAsync(h => h.Id == id);

        if (heliToUpdate == null)
        {
            throw new KeyNotFoundException($"Helicopter with id {id} was not found");
        }

        if (updatedHeli.TailNum != -1)
        {
            heliToUpdate.TailNum = updatedHeli.TailNum;
        }
        if (updatedHeli.Usability != null)
        {
            heliToUpdate.Usability = updatedHeli.Usability;
        }
        if (updatedHeli.FlightStatus != null)
        {
            heliToUpdate.FlightStatus = updatedHeli.FlightStatus;
        }

        await _db.SaveChangesAsync();
    }

    //Delete
    public async Task DeleteHeliByIdAsync(int id)
    {
        await _db.Helicopters.Where(h => h.Id == id).ExecuteDeleteAsync();

        await _db.SaveChangesAsync();
    }
}