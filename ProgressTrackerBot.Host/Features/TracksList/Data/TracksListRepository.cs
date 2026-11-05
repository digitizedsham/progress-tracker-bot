using Dapper;
using ProgressTrackerBot.Host.Common.DbConnectionFactory;
using ProgressTrackerBot.Host.Entities.Track;

namespace ProgressTrackerBot.Host.Features.TracksList.Data;

internal class TracksListRepository(IDbConnectionFactory dbConnectionFactory) : ITracksListRepository
{
    public async Task<int> GetTracksCount(long userId)
    {
        const string sql = """
                           SELECT COUNT(*)
                           FROM public.tracks
                           WHERE user_id = @UserId;
                           """;

        await using var connection = dbConnectionFactory.Create();

        return await connection.QuerySingleAsync<int>(
            sql,
            new { UserId = userId }
        );
    }
    
    public async Task<IEnumerable<Track>> GetTracksListByOffset(
        long userId,
        int offset,
        int limit = 10)
    {
        const string sql = $@"
                           SELECT
                               id AS {nameof(Track.Id)},
                               user_id AS {nameof(Track.UserId)},
                               name AS {nameof(Track.Name)},
                               weekdays_mask AS {nameof(Track.WeekdaysMask)},
                               sound_on AS {nameof(Track.SoundOn)},
                               reminder_hour AS {nameof(Track.ReminderHour)}
                           FROM public.tracks
                           WHERE user_id = @UserId
                           ORDER BY id
                           LIMIT @Limit
                           OFFSET @Offset;
                           ";

        await using var connection = dbConnectionFactory.Create();

        return await connection.QueryAsync<Track>(
            sql,
            new
            {
                UserId = userId,
                Offset = offset,
                Limit = limit
            });
    }
}