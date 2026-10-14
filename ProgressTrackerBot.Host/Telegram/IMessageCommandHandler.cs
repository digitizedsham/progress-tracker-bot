using Telegram.Bot.Types;

namespace ProgressTrackerBot.Host.Telegram;

internal interface IMessageCommandHandler
{
    public string CommandName { get; }

    public Task Handle(Message message, CancellationToken cancellationToken);
}