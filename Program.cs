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

builder.Services.AddValidation();

var app = builder.Build();

app.MapShiftEndpoints();
app.MapHelicopterGroupEndpoints();

app.Run();