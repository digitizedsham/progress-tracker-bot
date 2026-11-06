using Dapper;
using ProgressTrackerBot.Host.Common.DbConnectionFactory;
using ProgressTrackerBot.Host.Entities.Tracks;

namespace ProgressTrackerBot.Host.Notifications.Data;

internal class NotificationsSchedulerRepository(IDbConnectionFactory dbConnectionFactory) : INotificationsSchedulerRepository
{

    public async Task<IEnumerable<Track>> GetUnscheduledTrackNotifications(DateTime scheduledAt, short weekdayMask, short reminderHour, short limit = 100)
    {
        const string sql = $@"
                           SELECT
                               t.id AS {nameof(Track.Id)},
                               t.user_id AS {nameof(Track.UserId)},
                               t.name AS {nameof(Track.Name)},
                               t.weekdays_mask AS {nameof(Track.WeekdaysMask)},
                               t.sound_on AS {nameof(Track.SoundOn)},
                               t.reminder_hour AS {nameof(Track.ReminderHour)}
                           FROM public.tracks t
                           WHERE
                               t.reminder_hour = @ReminderHour
                               AND (t.weekdays_mask & @WeekdayMask) <> 0
                               AND NOT EXISTS (
                                   SELECT 1
                                   FROM public.notifications n
                                   WHERE
                                       n.track_id = t.id
                                       AND n.scheduled_at = @ScheduledAt
                               )
                           ORDER BY t.id
                           LIMIT @Limit;
                           ";

        await using var connection = dbConnectionFactory.Create();

        return await connection.QueryAsync<Track>(
            sql,
            new
            {
                WeekdayMask = weekdayMask,
                ReminderHour = reminderHour,
                ScheduledAt = scheduledAt,
                Limit = limit
            });
    }

    public async Task ScheduleTrackNotifications(DateTime scheduledAt, long[] trackIds, long[] userIds)
    {
        const string sql = @"
                           INSERT INTO public.notifications
                           (
                               track_id,
                               user_id,
                               scheduled_at
                           )
                           SELECT
                               track_id,
                               user_id,
                               @ScheduledAt
                           FROM UNNEST(
                               @TrackIds::BIGINT[],
                               @UserIds::BIGINT[]
                           ) AS batch(track_id, user_id)
                           ON CONFLICT (track_id, scheduled_at)
                           DO NOTHING;
                           ";

        await using var connection = dbConnectionFactory.Create();

        await connection.ExecuteAsync(
            sql,
            new
            {
                TrackIds = trackIds,
                UserIds = userIds,
                ScheduledAt = scheduledAt
            });
    }
}