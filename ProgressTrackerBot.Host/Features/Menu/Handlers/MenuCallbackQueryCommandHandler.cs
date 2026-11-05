using ProgressTrackerBot.Host.Common;
using ProgressTrackerBot.Host.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProgressTrackerBot.Host.Features.Menu.Handlers;

internal class MenuCallbackQueryCommandHandler(ITelegramBotClient bot) : ICallbackQueryCommandHandler
{
    public string CommandName => CommandNameConstants.MenuCommandName;

    public async Task Handle(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        await bot.EditMessageText(
            chatId: callbackQuery.From.Id,
            messageId: callbackQuery.Message!.MessageId,
            text: GetMessageText(),
            parseMode: ParseMode.Html,
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