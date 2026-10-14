using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace ProgressTrackerBot.Host.Telegram;

internal sealed class TelegramPollingService(
    ITelegramBotClient bot,
    UpdateDispatcher dispatcher,
    ILogger<TelegramPollingService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var me = await bot.GetMe(stoppingToken);

        logger.LogInformation(
            "Запуск бота @{Username}.",
            me.Username);
        
        await bot.ReceiveAsync(
            updateHandler: (client, update, cancellationToken) =>
                dispatcher.Handle(update, cancellationToken),
            errorHandler: (client, exception, cancellationToken) =>
            {
                logger.LogError(
                    exception,
                    "Произошла ошибка при полинге событий телеграма.");

                return Task.CompletedTask;
            },
            receiverOptions: new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery]
            },
            cancellationToken: stoppingToken);
    }
}