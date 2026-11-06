using ProgressTrackerBot.Host.Entities.Tracks;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProgressTrackerBot.Host.Common.TrackSettingsKeyboardFactory;

internal interface ITrackSettingsKeyboardFactory
{
    public InlineKeyboardMarkup Create(Track track);
}