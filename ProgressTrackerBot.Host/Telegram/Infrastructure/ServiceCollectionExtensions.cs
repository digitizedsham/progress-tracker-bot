using Telegram.Bot;

namespace ProgressTrackerBot.Host.Telegram.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddTelegram(this IServiceCollection services)
    {
        services.AddSingleton<ITelegramBotClient>(provider =>
        {
            return new TelegramBotClient("");
        });
        
        services.AddHostedService<TelegramPollingService>();
    }
}