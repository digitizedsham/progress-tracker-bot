using ProgressTrackerBot.Host.Features.TracksList.Data;
using ProgressTrackerBot.Host.Features.TracksList.Handlers;
using ProgressTrackerBot.Host.Telegram;

namespace ProgressTrackerBot.Host.Features.TracksList.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddTracksListFeature(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITracksListRepository, TracksListRepository>();
        
        services.AddScoped<ICallbackQueryCommandHandler, TracksListCallbackQueryCommandHandler>();
    }
}