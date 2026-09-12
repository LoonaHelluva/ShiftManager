using System;
using Microsoft.EntityFrameworkCore;

namespace HelipadManager;

public class ShiftDbContext : DbContext
{
    public ShiftDbContext(DbContextOptions<ShiftDbContext> options) : base(options) { }

    public DbSet<StaffMember> StaffMembers { get; set; }
    public DbSet<Helicopter> Helicopters { get; set; }
    public DbSet<HeliTask> Tasks { get; set; }
    public DbSet<Shift> Shifts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //HeliTask -> StaffMember
        modelBuilder.Entity<HeliTask>().
        HasMany(t => t.Executors).
        WithMany(s => s.Tasks).
        UsingEntity(j => j.ToTable("HeliTaskStaffMembers"));

        //Shift -> Helicopters
        modelBuilder.Entity<Shift>().
        HasMany(h => h.Helicopters).
        WithMany(s => s.Shifts).
        UsingEntity(t => t.ToTable("ShiftHelicopters"));
    }
}