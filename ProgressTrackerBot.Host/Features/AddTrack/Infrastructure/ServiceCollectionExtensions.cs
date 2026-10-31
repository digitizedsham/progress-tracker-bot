using ProgressTrackerBot.Host.Features.AddTrack.Handlers;
using ProgressTrackerBot.Host.Telegram;

namespace ProgressTrackerBot.Host.Features.AddTrack.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddAddTrackFeature(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICallbackQueryCommandHandler, AddTrackCallbackQueryCommandHandler>();
    }
}