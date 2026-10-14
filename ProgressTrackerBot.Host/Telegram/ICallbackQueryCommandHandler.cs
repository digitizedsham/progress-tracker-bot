using Telegram.Bot.Types;

namespace ProgressTrackerBot.Host.Telegram;

internal interface ICallbackQueryCommandHandler
{
    public string CommandName { get; }

    public Task Handle(CallbackQuery callbackQuery, CancellationToken cancellationToken);
}