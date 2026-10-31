using ProgressTrackerBot.Host.Common;
using ProgressTrackerBot.Host.Features.Start.Data;
using ProgressTrackerBot.Host.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProgressTrackerBot.Host.Features.Start.Handlers;

internal class StartMessageCommandHandler(ITelegramBotClient bot, IRegisterUserRepository registerUserRepository, ILogger<StartMessageCommandHandler> logger) : IMessageCommandHandler
{
    public string CommandName => CommandNameConstants.StartCommandName;

    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        await registerUserRepository.AddOrUpdate(message.Chat.Id, message.Chat.Username);
        
        await bot.SendMessage(
            chatId: message.Chat.Id,
            text: GetMessageText(),
            parseMode: ParseMode.Html,
            disableNotification: true,
            replyMarkup: GetMenuKeyboard(),
            cancellationToken: cancellationToken);
    }
    
    private static string GetMessageText()
    {
        return "Это бот, который помогает отслеживать прогресс по вашим целям, привычкам, рутине или хобби.";
    }
    
    private static InlineKeyboardMarkup GetMenuKeyboard()
    {
        return new InlineKeyboardMarkup([
            [
                InlineKeyboardButton.WithCallbackData(
                    text: CommandTitleConstants.AddNewTrackCommandTitle,
                    callbackData: CommandNameConstants.AddNewTrackCommandName)
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    text: CommandTitleConstants.TracksListCommandTitle,
                    callbackData: CommandNameConstants.TracksListCommandName)
            ]
        ]);
    }
}