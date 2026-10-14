using ProgressTrackerBot.Host.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProgressTrackerBot.Host.Features.Start.Handlers;

internal class StartMessageCommandHandler(ITelegramBotClient bot) : IMessageCommandHandler
{
    public string CommandName => "/start";

    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        await bot.SendMessage(
            chatId: message.Chat.Id, 
            text: "Привет!", 
            cancellationToken: cancellationToken);
    }
}