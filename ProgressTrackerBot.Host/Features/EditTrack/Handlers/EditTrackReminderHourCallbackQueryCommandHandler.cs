using ProgressTrackerBot.Host.Common;
using ProgressTrackerBot.Host.Entities.Tracks;
using ProgressTrackerBot.Host.Features.EditTrack.Data;
using ProgressTrackerBot.Host.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProgressTrackerBot.Host.Features.EditTrack.Handlers;

internal class EditTrackReminderHourCallbackQueryCommandHandler(
    ITelegramBotClient bot,
    IEditTrackRepository editTrackRepository) : ICallbackQueryCommandHandler
{
    public string CommandName => CommandNameConstants.EditTrackReminderHourCommandName;

    public async Task Handle(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var arguments = callbackQuery.Data!.Split(" ").Skip(1).ToArray();

        if (arguments.Length != 1)
        {
            await bot.EditMessageText(
                chatId: callbackQuery.From.Id,
                messageId: callbackQuery.Message!.MessageId,
                text: IncorrectArgumentsCommandMessageText(),
                parseMode: ParseMode.Html,
                replyMarkup: GetMenuKeyboard(),
                cancellationToken: cancellationToken);
            
            return;
        }
        
        var trackId = long.Parse(arguments[0]);
        var track = await editTrackRepository.GetTrack(trackId);
        if (track == null)
        {
            await bot.EditMessageText(
                chatId: callbackQuery.From.Id,
                messageId: callbackQuery.Message!.MessageId,
                text: TrackNotFoundCommandMessageText(),
                parseMode: ParseMode.Html,
                replyMarkup: GetMenuKeyboard(),
                cancellationToken: cancellationToken);
            
            return;
        }
        
        await bot.EditMessageText(
            chatId: callbackQuery.From.Id,
            messageId: callbackQuery.Message!.MessageId,
            text: TrackReminderHourSettingsMessageText(track.Name),
            parseMode: ParseMode.Html,
            replyMarkup: GetTrackReminderHourSettingsKeyboard(track),
            cancellationToken: cancellationToken);
    }

    private static string TrackReminderHourSettingsMessageText(string trackName)
    {
        return $"Выберите время (пока только по Мск), в которое вам было бы удобно получать напоминание о цели \"{trackName}\":";
    }

    private static InlineKeyboardMarkup GetTrackReminderHourSettingsKeyboard(Track track)
    {
        var buttons = new List<List<InlineKeyboardButton>>();

        var buttonRow = new List<InlineKeyboardButton>();
        for (int i = 0; i < 24; i++)
        {
            var checkSymbol = track.ReminderHour == i ? "✅ " : "";
            var nextHour = i == 23 ? 0 : i + 1;
            var text = $"{checkSymbol}{i:D2}:00-{nextHour:D2}:00";

            buttonRow.Add(
                InlineKeyboardButton.WithCallbackData(
                    text: text,
                    callbackData:
                    $"{CommandNameConstants.EditTrackSettingsCommandName} {track.Id} {track.WeekdaysMask} {track.SoundOn} {i}")
            );

            if (buttonRow.Count == 3)
            {
                buttons.Add(buttonRow);
                buttonRow = new List<InlineKeyboardButton>();
            }
        }

        buttons.Add([
            InlineKeyboardButton.WithCallbackData(
                text: $"« {CommandTitleConstants.EditTrackSettingsCommandTitle}",
                callbackData: $"{CommandNameConstants.EditTrackSettingsCommandName} {track.Id} {track.WeekdaysMask} {track.SoundOn} {track.ReminderHour}")
        ]);

        buttons.Add([
            InlineKeyboardButton.WithCallbackData(
                text: $"« {CommandTitleConstants.MenuCommandTitle}",
                callbackData: CommandNameConstants.MenuCommandName)
        ]);

        return buttons;
    }

    private static string IncorrectArgumentsCommandMessageText()
    {
        return "Что-то пошло не так. Попробуйте вернуться в главное меню.";
    }
    
    private static string TrackNotFoundCommandMessageText()
    {
        return "Что-то пошло не так, цель для редактирования не найдена. Попробуйте вернуться в главное меню.";
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