using Dapper;
using ProgressTrackerBot.Host.Common.DbConnectionFactory;
using ProgressTrackerBot.Host.Notifications.Data.Models;

namespace ProgressTrackerBot.Host.Notifications.Data;

internal class NotificationsSenderRepository(IDbConnectionFactory dbConnectionFactory) : INotificationsSenderRepository
{
    public async Task<IEnumerable<NotificationToSendModel>> GetNotificationsToSend(short limit = 100)
    {
        const string sql = $@"
                           WITH batch AS
                           (
                               SELECT id
                               FROM public.notifications
                               WHERE
                                   (
                                       status = 'pending'
                                       AND next_attempt_at <= NOW()
                                   )
                                   OR
                                   (
                                       status = 'processing'
                                       AND locked_until <= NOW()
                                   )
                               ORDER BY id
                               LIMIT @Limit
                               FOR UPDATE SKIP LOCKED
                           )
                           UPDATE public.notifications n
                           SET
                               status = 'processing',
                               locked_until = NOW() + INTERVAL '2 minutes',
                               attempts = attempts + 1
                           FROM batch
                           WHERE n.id = batch.id
                           RETURNING
                               n.id AS {nameof(NotificationToSendModel.Id)},
                               n.track_id AS {nameof(NotificationToSendModel.TrackId)},
                               n.user_id AS {nameof(NotificationToSendModel.UserId)},
                               n.scheduled_at AS {nameof(NotificationToSendModel.ScheduledAt)},
                               n.attempts AS {nameof(NotificationToSendModel.Attempts)};
                           ";

        await using var connection = dbConnectionFactory.Create();

        return await connection.QueryAsync<NotificationToSendModel>(
            sql,
            new
            {
                Limit = limit
            });
    }

    public async Task MarkNotificationAsSent(long notificationId)
    {
        const string sql = """
                           UPDATE public.notifications
                           SET
                               status = 'sent',
                               sent_at = NOW(),
                               locked_until = NULL
                           WHERE id = @NotificationId;
                           """;

        await using var connection = dbConnectionFactory.Create();

        await connection.ExecuteAsync(sql, new
        {
            NotificationId = notificationId
        });
    }

    public async Task MarkNotificationAsFailed(long notificationId, string errorMessage)
    {
        const string sql = """
                           UPDATE public.notifications
                           SET
                               status = 'failed',
                               last_error = @ErrorMessage
                           WHERE id = @NotificationId;
                           """;

        await using var connection = dbConnectionFactory.Create();

        await connection.ExecuteAsync(sql, new
        {
            NotificationId = notificationId,
            ErrorMessage = errorMessage
        });
    }
}