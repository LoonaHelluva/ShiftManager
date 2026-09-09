using HelipadManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShiftDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("LocalDbConnection"));
});

builder.Services.AddScoped<IShiftDbService, ShiftDbService>();
builder.Services.AddScoped<IHelicopterDbService, HelicopterDbService>();
builder.Services.AddScoped<ITaskDbService, TaskDbService>();

builder.Services.AddValidation();

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    ShiftDbContext db = scope.ServiceProvider.GetRequiredService<ShiftDbContext>();
    db.Database.Migrate();
}

app.MapShiftEndpoints();
app.MapHelicopterGroupEndpoints();
app.MapTaskEndpoints();

app.Run();