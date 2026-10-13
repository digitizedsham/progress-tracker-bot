using Telegram.Bot;
using Telegram.Bot.Polling;

namespace ProgressTrackerBot.Host.Telegram;

internal sealed class TelegramPollingService(
    ITelegramBotClient bot,
    ILogger<TelegramPollingService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var me = await bot.GetMe(stoppingToken);

        logger.LogInformation(
            "Starting Telegram bot @{Username}",
            me.Username);
        
        await bot.ReceiveAsync(
            updateHandler: (client, update, cancellationToken) =>
            {
                // тут будет обработчик входящих update'ов.
                return Task.CompletedTask;
            },
            errorHandler: (client, exception, cancellationToken) =>
            {
                logger.LogError(
                    exception,
                    "Telegram polling error.");

                return Task.CompletedTask;
            },
            receiverOptions: new ReceiverOptions
            {
                AllowedUpdates = []
            },
            cancellationToken: stoppingToken);
    }
}