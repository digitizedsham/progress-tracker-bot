using ProgressTrackerBot.Host.Common;
using ProgressTrackerBot.Host.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProgressTrackerBot.Host.Features.Menu.Handlers;

internal class NewMessageMenuCallbackQueryCommandHandler(ITelegramBotClient bot) : ICallbackQueryCommandHandler
{
    public string CommandName => CommandNameConstants.NewMessageMenuCommandName;

    public async Task Handle(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        await bot.SendMessage(
            chatId: callbackQuery.From.Id,
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
                    text: CommandTitleConstants.AddTrackCommandTitle,
                    callbackData: CommandNameConstants.AddTrackCommandName)
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    text: CommandTitleConstants.TracksListCommandTitle,
                    callbackData: $"{CommandNameConstants.TracksListCommandName} {0}")
            ]
        ]);
    }
}