using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProgressTrackerBot.Host.Telegram.UpdateHandlers;

internal interface IUpdateHandler
{
    public UpdateType UpdateType { get; }

    public Task<UpdateHandleResult?> Handle(Update update, CancellationToken cancellationToken);
}