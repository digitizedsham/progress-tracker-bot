using Telegram.Bot;

namespace ProgressTrackerBot.Host.Telegram.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddTelegram(this IServiceCollection services)
    {
        services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(""));
        services.AddHostedService<TelegramPollingService>();
        services.AddSingleton<UpdateDispatcher>();
        services.AddSingleton<ICommandExtractor, CommandExtractor>();
    }
}