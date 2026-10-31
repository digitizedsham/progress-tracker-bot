using ProgressTrackerBot.Host.Telegram.CommandExtractor;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProgressTrackerBot.Host.Telegram.UpdateHandlers;

internal class CallbackQueryUpdateHandler(
    ITelegramBotClient bot,
    ICommandExtractor commandExtractor,
    IEnumerable<ICallbackQueryCommandHandler> callbackQueryCommandHandlers,
    ILogger<UpdateDispatcher> logger) : IUpdateHandler
{
    public UpdateType UpdateType => UpdateType.CallbackQuery;

    public async Task<UpdateHandleResult?> Handle(Update update, CancellationToken cancellationToken)
    {
        try
        {
            if (update.CallbackQuery == null)
            {
                logger.LogError("CallbackQuery не может быть null.");
                return null;
            }

            var command = commandExtractor.Extract(update.CallbackQuery.Data);
            var callbackQueryCommandHandler =
                callbackQueryCommandHandlers.SingleOrDefault(c => c.CommandName == command);

            if (callbackQueryCommandHandler == null)
            {
                logger.LogError("Не найден обработчик CallbackQuery для команды '{Command}'.", command);
                return null;
            }

            await callbackQueryCommandHandler.Handle(update.CallbackQuery, cancellationToken);

            return new UpdateHandleResult
            {
                UserId = update.CallbackQuery.From.Id,
                Command =  command,
            };
        }
        finally
        {
            if (update.CallbackQuery != null)
            {
                await bot.AnswerCallbackQuery(
                    update.CallbackQuery.Id,
                    cancellationToken: cancellationToken);
            }
        }
    }
}