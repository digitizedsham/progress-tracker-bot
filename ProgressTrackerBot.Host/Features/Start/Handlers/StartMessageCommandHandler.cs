using ProgressTrackerBot.Host.Common;
using ProgressTrackerBot.Host.Features.Start.Data;
using ProgressTrackerBot.Host.Features.Start.Data.Models;
using ProgressTrackerBot.Host.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProgressTrackerBot.Host.Features.Start.Handlers;

internal class StartMessageCommandHandler(
    ITelegramBotClient bot,
    IRegisterUserRepository registerUserRepository) : IMessageCommandHandler
{
    public string CommandName => CommandNameConstants.StartCommandName;

    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        await registerUserRepository.AddOrUpdate(
            new AddUserModel
            {
                Id = message.Chat.Id,
                Username = message.Chat.Username
            });
        
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
                    text: CommandTitleConstants.AddTrackCommandTitle,
                    callbackData: CommandNameConstants.AddTrackCommandName)
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    text: CommandTitleConstants.TracksListCommandTitle,
                    callbackData: CommandNameConstants.TracksListCommandName)
            ]
        ]);
    }
}