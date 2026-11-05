using ProgressTrackerBot.Host.Features.AddTrack.Infrastructure;
using ProgressTrackerBot.Host.Features.EditTrack.Infrastructure;
using ProgressTrackerBot.Host.Features.Menu.Infrastructure;
using ProgressTrackerBot.Host.Features.Start.Infrastructure;
using ProgressTrackerBot.Host.Features.TracksList.Infrastructure;

namespace ProgressTrackerBot.Host.Features.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddFeatures(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStartFeature(configuration);
        services.AddMenuFeature(configuration);
        services.AddAddTrackFeature(configuration);
        services.AddEditTrackFeature(configuration);
        services.AddTracksListFeature(configuration);
    }
}