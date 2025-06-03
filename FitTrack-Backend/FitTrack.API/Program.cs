using FitTrack.API.Infrastructure;
using FitTrack.API.Infrastructure.Middleware;
using FitTrack.Data.Access.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config
        .WriteTo.Console()
        .WriteTo.File("logs/app-log.txt", rollingInterval: RollingInterval.Day)
        .Enrich.FromLogContext();
});

builder.Services.AddDbContext<FitTrackContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddServices();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Origins");

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();

