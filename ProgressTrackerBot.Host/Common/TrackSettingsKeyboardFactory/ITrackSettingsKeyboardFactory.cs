using ProgressTrackerBot.Host.Entities.Track;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProgressTrackerBot.Host.Common.TrackSettingsKeyboardFactory;

internal interface ITrackSettingsKeyboardFactory
{
    public InlineKeyboardMarkup Create(Track track);
}