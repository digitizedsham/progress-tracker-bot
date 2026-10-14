using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProgressTrackerBot.Host.Telegram;

internal sealed class UpdateDispatcher(
    IServiceScopeFactory scopeFactory,
    ITelegramBotClient bot,
    ICommandExtractor commandExtractor,
    IEnumerable<IMessageCommandHandler> messageCommandHandlers,
    IEnumerable<ICallbackQueryCommandHandler> callbackQueryCommandHandlers,
    ILogger<UpdateDispatcher> logger)
{
    public async Task Handle(
        Update update,
        CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();

            if (update.Type == UpdateType.Message)
            {
                if (update.Message == null)
                {
                    logger.LogError("Сообщение не может быть null.");
                    return;
                }

                var command = commandExtractor.Extract(update.Message.Text);
                var messageCommandHandler = messageCommandHandlers.FirstOrDefault(c => c.CommandName == command);

                if (messageCommandHandler == null)
                {
                    logger.LogError("Не найден обработчик сообщения для команды '{Command}'.", command);
                    return;
                }
                
                await messageCommandHandler.Handle(update.Message, cancellationToken);
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                if (update.CallbackQuery == null)
                {
                    logger.LogError("CallbackQuery не может быть null.");
                    return;
                }

                var command = commandExtractor.Extract(update.CallbackQuery.Data);
                var callbackQueryCommandHandler = callbackQueryCommandHandlers.FirstOrDefault(c => c.CommandName == command);

                if (callbackQueryCommandHandler == null)
                {
                    logger.LogError("Не найден обработчик CallbackQuery для команды '{Command}'.", command);
                    return;
                }
                
                await callbackQueryCommandHandler.Handle(update.CallbackQuery, cancellationToken);
            }

            logger.LogError("Тип события {UpdateType} не поддерживается.", update.Type.ToString());

        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка обработки события из телеграма.");
        }
    }
}