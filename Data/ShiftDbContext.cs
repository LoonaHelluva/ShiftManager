using System;
using Microsoft.EntityFrameworkCore;

namespace HelipadManager;

public class ShiftDbContext : DbContext
{
    public ShiftDbContext(DbContextOptions<ShiftDbContext> options) : base(options) { }

    public DbSet<StaffMember> StaffMembers { get; set; }
    public DbSet<Helicopter> Helicopters { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<SubTask> SubTasks { get; set; }
    public DbSet<Shift> Shifts { get; set; }
}