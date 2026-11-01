using ProgressTrackerBot.Host.Common;
using ProgressTrackerBot.Host.Common.LastCommandRepository;
using ProgressTrackerBot.Host.Common.TrackSettingsKeyboardFactory;
using ProgressTrackerBot.Host.Features.AddTrack.Data;
using ProgressTrackerBot.Host.Features.AddTrack.Data.Models;
using ProgressTrackerBot.Host.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProgressTrackerBot.Host.Features.AddTrack.Handlers;

internal class AddNameOfTrackMessageCommandHandler(
    ITelegramBotClient bot, 
    ILastCommandRepository lastCommandRepository,
    IAddTrackRepository addTrackRepository,
    ITrackSettingsKeyboardFactory trackSettingsKeyboardFactory) : IMessageCommandHandler
{
    public string CommandName => string.Empty;

    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var lastCommand = await lastCommandRepository.GetLastCommand(message.Chat.Id);
        if (lastCommand is not { Command: CommandNameConstants.AddTrackCommandName })
        {
            await bot.SendMessage(
                chatId: message.Chat.Id,
                text: IncorrectLastCommandMessageText(),
                parseMode: ParseMode.Html,
                disableNotification: true,
                replyMarkup: GetMenuKeyboard(),
                cancellationToken: cancellationToken);
            
            return;
        }

        if (string.IsNullOrWhiteSpace(message.Text))
        {
            await bot.SendMessage(
                chatId: message.Chat.Id,
                text: EmptyTrackNameMessageText(),
                parseMode: ParseMode.Html,
                disableNotification: true,
                replyMarkup: GetMenuKeyboard(),
                cancellationToken: cancellationToken);
            
            return;
        }

        if (message.Text.Length > 150)
        {
            await bot.SendMessage(
                chatId: message.Chat.Id,
                text: TooLongTrackNameMessageText(),
                parseMode: ParseMode.Html,
                disableNotification: true,
                replyMarkup: GetMenuKeyboard(),
                cancellationToken: cancellationToken);
            
            return;
        }

        var track = await addTrackRepository.AddTrack(new AddTrackNameModel
        {
            UserId = message.Chat.Id,
            Name = message.Text
        });
        
        await bot.SendMessage(
            chatId: message.Chat.Id,
            text: TrackSettingsMessageText(track.Name),
            parseMode: ParseMode.Html,
            disableNotification: true,
            replyMarkup: trackSettingsKeyboardFactory.Create(track),
            cancellationToken: cancellationToken);
    }

    private static string IncorrectLastCommandMessageText()
    {
        // TODO: продумать текст ответа.
        return "Неизвестная команда. Попробуйте вернуться в главное меню.";
    }

    private static string EmptyTrackNameMessageText()
    {
        return "Название цели не должно состоять только из одних пробелов. Попробуйте ввести другое название:";
    }

    private static string TooLongTrackNameMessageText()
    {
        return "Длина названия цели не должна превышать 150 символов. Попробуйте ввести другое название:";
    }

    private static string TrackSettingsMessageText(string trackName)
    {
        return $"Выберите в какие дни недели и в какое время присылать вам оповещения по цели \"{trackName}\":";
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