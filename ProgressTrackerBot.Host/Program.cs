using ProgressTrackerBot.Host.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAllServices(builder.Configuration);

var app = builder.Build();

app.Run();