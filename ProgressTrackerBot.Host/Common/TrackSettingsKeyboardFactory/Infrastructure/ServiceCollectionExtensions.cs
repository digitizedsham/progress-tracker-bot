namespace ProgressTrackerBot.Host.Common.TrackSettingsKeyboardFactory.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddTrackSettingsKeyboardFactory(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITrackSettingsKeyboardFactory, TrackSettingsKeyboardFactory>();
    }
}