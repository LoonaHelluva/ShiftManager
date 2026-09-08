using System;
using Microsoft.EntityFrameworkCore;

namespace HelipadManager;

public class HelicopterDbService : IHelicopterDbService
{
    private ShiftDbContext _db;
    public HelicopterDbService(ShiftDbContext db)
    {
        _db = db;
    }

    //Create
    public async Task<int> AddHeliAsync(AddHeliDto helicopterDto)
    {
        Helicopter newHelicopter = new(
                                        helicopterDto.Usability,
                                        helicopterDto.FlightStatus,
                                        helicopterDto.TailNum);

        await _db.Helicopters.AddAsync(newHelicopter);

        return newHelicopter.Id;
    }

    //Read
    public async Task<GetHeliDto?> GetHeliByIdAsync(int id)
    {
        var helicopter = await _db.Helicopters.FirstOrDefaultAsync(h => h.Id == id);

        if (helicopter is null)
        {
            return null;
        }

        GetHeliDto returnHeli = new GetHeliDto(
                                                helicopter.Id,
                                                helicopter.TailNum,
                                                helicopter.Usability,
                                                helicopter.FlightStatus,
                                                new List<Task>(),
                                                new List<Shift>()
        );

        return returnHeli;
    }
    public async Task<GetHeliDto> GetHeliWithListsByIdAsync(int id)
    {
        var helicopter = await _db.Helicopters.FirstOrDefaultAsync(h => h.Id == id);

        if (helicopter is null)
        {
            return null;
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
    public async Task<List<GetHeliDto?>?> GetHelisAsync()
    {
        List<Helicopter>? helis = await _db.Helicopters.AsNoTracking().ToListAsync();

        if (helis is not null)
        {
            return null;
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

    //Delete
}
