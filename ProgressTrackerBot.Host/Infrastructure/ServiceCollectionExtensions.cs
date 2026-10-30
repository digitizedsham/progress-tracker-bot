using ProgressTrackerBot.Host.Features.Infrastructure;
using ProgressTrackerBot.Host.Telegram.Infrastructure;
using Serilog;

namespace ProgressTrackerBot.Host.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddAllServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog();
        services.AddTelegram(configuration);
        services.AddFeatures(configuration);
    }
}