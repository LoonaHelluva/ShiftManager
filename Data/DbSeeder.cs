using Microsoft.EntityFrameworkCore;

namespace HelipadManager;

public static class DbSeeder
{
    public static async Task SeedAsync(ShiftDbContext db)
    {
        List<StaffMember> staff = await db.StaffMembers.OrderBy(s => s.Id).Take(3).ToListAsync();
        StaffMember? manager = staff.FirstOrDefault(s => s.IsManager);

        if (manager is null)
        {
            manager = new StaffMember("Иван Петров", 1001, true);
            staff.Add(manager);
            await db.StaffMembers.AddAsync(manager);
        }

        while (staff.Count < 3)
        {
            StaffMember newStaff = staff.Count == 1
                ? new StaffMember("Анна Смирнова", 1002, false)
                : new StaffMember("Михаил Орлов", 1003, false);

            staff.Add(newStaff);
            await db.StaffMembers.AddAsync(newStaff);
        }

        List<Helicopter> helicopters = await db.Helicopters.OrderBy(h => h.Id).Take(2).ToListAsync();

        while (helicopters.Count < 2)
        {
            Helicopter newHelicopter = helicopters.Count == 0
                ? new Helicopter("Исправен", "На базе", 101)
                : new Helicopter("Исправен", "В полете", 102);

            helicopters.Add(newHelicopter);
            await db.Helicopters.AddAsync(newHelicopter);
        }

        await db.SaveChangesAsync();

        StaffMember engineer = staff[1];
        StaffMember technician = staff[2];
        Helicopter firstHelicopter = helicopters[0];
        Helicopter secondHelicopter = helicopters[1];
        List<Shift> shifts = await db.Shifts.OrderBy(s => s.Id).Take(2).ToListAsync();

        while (shifts.Count < 2)
        {
            Shift newShift = shifts.Count == 0
                ? new Shift(DateOnly.FromDateTime(DateTime.UtcNow.Date), manager.Id)
                : new Shift(DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(1)), manager.Id);

            if (shifts.Count == 0)
            {
                newShift.StaffMembers.Add(manager);
                newShift.StaffMembers.Add(engineer);
                newShift.Helicopters.Add(firstHelicopter);
            }
            else
            {
                newShift.StaffMembers.Add(technician);
                newShift.Helicopters.Add(secondHelicopter);
            }

            shifts.Add(newShift);
            await db.Shifts.AddAsync(newShift);
            await db.SaveChangesAsync();
        }

        List<HeliTask> tasks = await db.Tasks.OrderBy(t => t.Id).Take(2).ToListAsync();

        while (tasks.Count < 2)
        {
            HeliTask newTask = tasks.Count == 0
                ? new HeliTask(
                    "Предполетный осмотр",
                    "Проверить основные системы вертолета перед вылетом.",
                    shifts[0].Id,
                    firstHelicopter.Id,
                    new List<StaffMember> { engineer })
                : new HeliTask(
                    "Плановое обслуживание",
                    "Выполнить плановое техническое обслуживание.",
                    shifts[1].Id,
                    secondHelicopter.Id,
                    new List<StaffMember> { technician });

            tasks.Add(newTask);
            await db.Tasks.AddAsync(newTask);
            await db.SaveChangesAsync();
        }

    }
}