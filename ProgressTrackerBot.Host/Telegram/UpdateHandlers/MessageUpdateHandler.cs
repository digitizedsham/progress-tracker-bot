using ProgressTrackerBot.Host.Telegram.CommandExtractor;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProgressTrackerBot.Host.Telegram.UpdateHandlers;

internal class MessageUpdateHandler(
    ICommandExtractor commandExtractor,
    IEnumerable<IMessageCommandHandler> messageCommandHandlers,
    ILogger<UpdateDispatcher> logger) : IUpdateHandler
{
    public UpdateType UpdateType => UpdateType.Message;

    public Task Handle(Update update, CancellationToken cancellationToken)
    {
        if (update.Message == null)
        {
            logger.LogError("Сообщение не может быть null.");
            return Task.CompletedTask;
        }

        var command = commandExtractor.Extract(update.Message.Text);
        var messageCommandHandler = messageCommandHandlers.SingleOrDefault(c => c.CommandName == command);

        if (messageCommandHandler == null)
        {
            logger.LogError("Не найден обработчик сообщения для команды '{Command}'.", command);
            return Task.CompletedTask;
        }
                
        return messageCommandHandler.Handle(update.Message, cancellationToken);
    }
}