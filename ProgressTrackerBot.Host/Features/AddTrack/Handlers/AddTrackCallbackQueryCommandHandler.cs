using ProgressTrackerBot.Host.Common;
using ProgressTrackerBot.Host.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProgressTrackerBot.Host.Features.AddTrack.Handlers;

internal class AddTrackCallbackQueryCommandHandler(ITelegramBotClient bot) : ICallbackQueryCommandHandler
{
    public string CommandName => CommandNameConstants.AddTrackCommandName;

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
        return "Пришлите название для вашей новой цели:";
    }
    
    private static InlineKeyboardMarkup GetMenuKeyboard()
    {
        return new InlineKeyboardMarkup([
            [
                InlineKeyboardButton.WithCallbackData(
                    text: $"« {CommandTitleConstants.MenuCommandTitle}",
                    callbackData: CommandNameConstants.MenuCommandName)
            ]
        ]);
    }
}