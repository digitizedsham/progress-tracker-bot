using Dapper;
using ProgressTrackerBot.Host.Common.DbConnectionFactory;

namespace ProgressTrackerBot.Host.Notifications.Data;

internal class NotificationsCleanerRepository(IDbConnectionFactory dbConnectionFactory) : INotificationsCleanerRepository
{
    public async Task<int> RemoveOutdatedNotifications(DateTime boundaryDate, int limit = 100)
    {
        const string sql = @"
                           WITH batch AS (
                               SELECT id
                               FROM public.notifications
                               WHERE scheduled_at < @BoundaryDate
                               ORDER BY scheduled_at
                               LIMIT @Limit
                           )
                           DELETE FROM public.notifications n
                           USING batch
                           WHERE n.id = batch.id;
                           ";

        await using var connection = dbConnectionFactory.Create();

        return await connection.ExecuteAsync(sql, new
        {
            BoundaryDate = boundaryDate,
            Limit = limit
        });
    }
}