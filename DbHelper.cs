using System;

namespace HelipadManager;

public class DbHelper
{
    private ShiftDbContext _db;
    public DbHelper(ShiftDbContext db)
    {
        _db = db;
    }
}
