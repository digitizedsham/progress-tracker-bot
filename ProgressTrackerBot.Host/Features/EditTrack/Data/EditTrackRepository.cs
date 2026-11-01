using Dapper;
using ProgressTrackerBot.Host.Common.DbConnectionFactory;
using ProgressTrackerBot.Host.Entities.Track;
using ProgressTrackerBot.Host.Features.EditTrack.Data.Models;

namespace ProgressTrackerBot.Host.Features.EditTrack.Data;

internal class EditTrackRepository(IDbConnectionFactory dbConnectionFactory) : IEditTrackRepository
{
    public async Task<Track?> EditTrackSettings(EditTrackSettingsModel model)
    {
        const string sql = $@"
                           UPDATE public.tracks
                           SET
                               weekdays_mask = @{nameof(EditTrackSettingsModel.WeekdaysMask)},
                               sound_on = @{nameof(EditTrackSettingsModel.SoundOn)},
                               reminder_hour = @{nameof(EditTrackSettingsModel.ReminderHour)}
                           WHERE id = @{nameof(EditTrackSettingsModel.Id)}
                           RETURNING 
                           id AS {nameof(Track.Id)}, 
                           user_id AS {nameof(Track.UserId)}, 
                           name AS {nameof(Track.Name)},
                           weekdays_mask AS {nameof(Track.WeekdaysMask)},
                           sound_on AS {nameof(Track.SoundOn)},
                           reminder_hour AS {nameof(Track.ReminderHour)};
                           ";

        await using var connection = dbConnectionFactory.Create();

        return await connection.QuerySingleOrDefaultAsync<Track>(sql, model);
    }
}