using ProgressTrackerBot.Host.Features.Infrastructure;
using ProgressTrackerBot.Host.Telegram.Infrastructure;

namespace ProgressTrackerBot.Host.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddAllServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTelegram(configuration);
        services.AddFeatures(configuration);
    }
}