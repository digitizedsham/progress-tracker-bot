using ProgressTrackerBot.Host.Features.AddTrack.Infrastructure;
using ProgressTrackerBot.Host.Features.Start.Infrastructure;

namespace ProgressTrackerBot.Host.Features.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddFeatures(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStartFeature(configuration);
        services.AddAddTrackFeature(configuration);
    }
}