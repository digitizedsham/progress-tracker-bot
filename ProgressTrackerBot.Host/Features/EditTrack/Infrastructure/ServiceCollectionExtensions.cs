using ProgressTrackerBot.Host.Features.EditTrack.Data;
using ProgressTrackerBot.Host.Features.EditTrack.Handlers;
using ProgressTrackerBot.Host.Telegram;

namespace ProgressTrackerBot.Host.Features.EditTrack.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddEditTrackFeature(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEditTrackRepository, EditTrackRepository>();

        services.AddScoped<ICallbackQueryCommandHandler, EditTrackSettingsCallbackQueryCommandHandler>();
    }
}