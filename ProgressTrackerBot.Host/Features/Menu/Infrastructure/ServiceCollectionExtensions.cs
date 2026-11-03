using ProgressTrackerBot.Host.Features.Menu.Handlers;
using ProgressTrackerBot.Host.Telegram;

namespace ProgressTrackerBot.Host.Features.Menu.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddMenuFeature(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICallbackQueryCommandHandler, MenuCallbackQueryCommandHandler>();
    }
}