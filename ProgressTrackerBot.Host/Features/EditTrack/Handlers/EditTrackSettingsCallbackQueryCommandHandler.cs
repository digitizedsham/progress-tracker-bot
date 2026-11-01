using ProgressTrackerBot.Host.Common;
using ProgressTrackerBot.Host.Common.TrackSettingsKeyboardFactory;
using ProgressTrackerBot.Host.Features.EditTrack.Data;
using ProgressTrackerBot.Host.Features.EditTrack.Data.Models;
using ProgressTrackerBot.Host.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProgressTrackerBot.Host.Features.EditTrack.Handlers;

internal class EditTrackSettingsCallbackQueryCommandHandler(
    ITelegramBotClient bot,
    IEditTrackRepository editTrackRepository,
    ITrackSettingsKeyboardFactory trackSettingsKeyboardFactory) : ICallbackQueryCommandHandler
{
    public string CommandName => CommandNameConstants.EditTrackSettingsCommandName;

    public async Task Handle(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var arguments = callbackQuery.Data!.Split(" ").Skip(1).ToArray();

        if (arguments.Length != 4)
        {
            await bot.SendMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                text: IncorrectArgumentsCommandMessageText(),
                parseMode: ParseMode.Html,
                disableNotification: true,
                replyMarkup: GetMenuKeyboard(),
                cancellationToken: cancellationToken);
            
            return;
        }

        var track = await editTrackRepository.EditTrackSettings(new EditTrackSettingsModel
        {
            Id = long.Parse(arguments[0]),
            WeekdaysMask = short.Parse(arguments[1]),
            SoundOn = bool.Parse(arguments[2]),
            ReminderHour = short.Parse(arguments[3]),
        });

        if (track == null)
        {
            await bot.SendMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                text: IncorrectArgumentsCommandMessageText(),
                parseMode: ParseMode.Html,
                disableNotification: true,
                replyMarkup: GetMenuKeyboard(),
                cancellationToken: cancellationToken);
            
            return;
        }

        await bot.EditMessageText(
            chatId: callbackQuery.From.Id,
            messageId: callbackQuery.Message!.MessageId,
            text: TrackSettingsMessageText(track.Name),
            parseMode: ParseMode.Html,
            replyMarkup: trackSettingsKeyboardFactory.Create(track),
            cancellationToken: cancellationToken);

    }
    
    private static string IncorrectArgumentsCommandMessageText()
    {
        return "Что-то пошло не так. Попробуйте вернуться в главное меню.";
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