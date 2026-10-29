using ProgressTrackerBot.Host.Telegram.CommandExtractor;
using ProgressTrackerBot.Host.Telegram.UpdateHandlers;
using Telegram.Bot;

namespace ProgressTrackerBot.Host.Telegram.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddTelegram(this IServiceCollection services, IConfiguration configuration)
    {
        var token = Environment.GetEnvironmentVariable(
            "TELEGRAM_BOT_TOKEN");

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException(
                "TELEGRAM_BOT_TOKEN is not configured.");
        }

        services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(token));
        services.AddHostedService<TelegramPollingService>();
        services.AddSingleton<UpdateDispatcher>();
        services.AddSingleton<ICommandExtractor, CommandExtractor.CommandExtractor>();
        
        services.AddScoped<IUpdateHandler, MessageUpdateHandler>();
        services.AddScoped<IUpdateHandler, CallbackQueryUpdateHandler>();
        
    }
}