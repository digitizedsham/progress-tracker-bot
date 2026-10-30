using ProgressTrackerBot.Host.Infrastructure;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddAllServices(builder.Configuration);

    var app = builder.Build();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Работа приложения неожиданно прервалась.");
}
finally
{
    Log.CloseAndFlush();
}