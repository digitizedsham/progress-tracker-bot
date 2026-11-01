using ProgressTrackerBot.Host.Common.DbConnectionFactory.Infrastructure;
using ProgressTrackerBot.Host.Common.LastCommandRepository.Infrastructure;
using ProgressTrackerBot.Host.Common.TrackSettingsKeyboardFactory.Infrastructure;

namespace ProgressTrackerBot.Host.Common.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddCommon(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbConnectionFactory(configuration);
        services.AddLastCommandRepository(configuration);
        services.AddTrackSettingsKeyboardFactory(configuration);
    }
}