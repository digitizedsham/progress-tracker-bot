using ProgressTrackerBot.Host.Entities.Track;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProgressTrackerBot.Host.Common.TrackSettingsKeyboardFactory;

internal class TrackSettingsKeyboardFactory : ITrackSettingsKeyboardFactory
{
    public InlineKeyboardMarkup Create(Track track)
    {
        var mondayText = (track.WeekdaysMask & (1 << 0)) != 0 ? "Понедельник ✅" : "Понедельник";
        var tuesdayText = (track.WeekdaysMask & (1 << 1)) != 0 ? "Вторник ✅" : "Вторник";
        var wednesdayText = (track.WeekdaysMask & (1 << 2)) != 0 ? "Среда ✅" : "Среда";
        var thursdayText = (track.WeekdaysMask & (1 << 3)) != 0 ? "Четверг ✅" : "Четверг";
        var fridayText = (track.WeekdaysMask & (1 << 4)) != 0 ? "Пятница ✅" : "Пятница";
        var saturdayText = (track.WeekdaysMask & (1 << 5)) != 0 ? "Суббота ✅" : "Суббота";
        var sundayText = (track.WeekdaysMask & (1 << 6)) != 0 ? "Воскресенье ✅" : "Воскресенье";

        var soundOffText = !track.SoundOn ? "🔇Без звука ✅" : "🔇Без звука";
        var soundOnText = track.SoundOn ? "🔈Со звуком ✅" : "🔈Со звуком";

        var endPeriodText = track.ReminderHour == 23 ? "00" : $"{track.ReminderHour + 1:D2}";
        var reminderPeriodText = $"🕔Время оповещения (Мск): {track.ReminderHour:D2}:00-{endPeriodText}:00";

        return new InlineKeyboardMarkup([
            [
                InlineKeyboardButton.WithCallbackData(
                    text: mondayText,
                    callbackData: GetCallbackDataToWeekDay(track, 0))
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    text: tuesdayText,
                    callbackData: GetCallbackDataToWeekDay(track, 1))
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    text: wednesdayText,
                    callbackData: GetCallbackDataToWeekDay(track, 2))
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    text: thursdayText,
                    callbackData: GetCallbackDataToWeekDay(track, 3))
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    text: fridayText,
                    callbackData: GetCallbackDataToWeekDay(track, 4))
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    text: saturdayText,
                    callbackData: GetCallbackDataToWeekDay(track, 5))
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    text: sundayText,
                    callbackData: GetCallbackDataToWeekDay(track, 6))
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    text: reminderPeriodText,
                    callbackData: GetCallbackDataToReminderHour(track)),
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    text: soundOffText,
                    callbackData: GetCallbackDataToSound(track, false)),
                InlineKeyboardButton.WithCallbackData(
                    text: soundOnText,
                    callbackData: GetCallbackDataToSound(track, true))

            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    text: $"« {CommandTitleConstants.MenuCommandTitle}",
                    callbackData: CommandNameConstants.MenuCommandName)
            ]
        ]);
    }

    private static string GetCallbackDataToSound(Track track, bool soundOn)
    {
        return
            $"{CommandNameConstants.EditTrackSettingsCommandName} {track.Id} {track.WeekdaysMask} {soundOn} {track.ReminderHour}";
    }

    private static string GetCallbackDataToReminderHour(Track track)
    {
        return $"{CommandNameConstants.EditTrackReminderHourCommandName} {track.Id}";
    }

    private static string GetCallbackDataToWeekDay(Track track, short weekday)
    {
        var newWeekdayMask = (short)(track.WeekdaysMask ^ (1 << weekday));
        return $"{CommandNameConstants.EditTrackSettingsCommandName} {track.Id} {newWeekdayMask} {track.SoundOn} {track.ReminderHour}";
    }
}