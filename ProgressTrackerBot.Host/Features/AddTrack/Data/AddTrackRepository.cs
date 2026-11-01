using Dapper;
using ProgressTrackerBot.Host.Common.DbConnectionFactory;
using ProgressTrackerBot.Host.Entities.Track;
using ProgressTrackerBot.Host.Features.AddTrack.Data.Models;

namespace ProgressTrackerBot.Host.Features.AddTrack.Data;

internal class AddTrackRepository(IDbConnectionFactory dbConnectionFactory) : IAddTrackRepository
{
    public async Task<Track> AddTrack(AddTrackNameModel model)
    {
        const string sql = $@"
                           INSERT INTO public.tracks (
                               user_id,
                               name             
                           )
                           VALUES (
                               @{nameof(AddTrackNameModel.UserId)},
                               @{nameof(AddTrackNameModel.Name)}
                           )
                           RETURNING 
                           id AS {nameof(Track.Id)}, 
                           user_id AS {nameof(Track.UserId)}, 
                           name AS {nameof(Track.Name)},
                           weekdays_mask AS {nameof(Track.WeekdaysMask)},
                           sound_on AS {nameof(Track.SoundOn)},
                           reminder_hour AS {nameof(Track.ReminderHour)};
                           ";

        await using var connection = dbConnectionFactory.Create();

        return await connection.QuerySingleAsync<Track>(sql, model);
    }
}