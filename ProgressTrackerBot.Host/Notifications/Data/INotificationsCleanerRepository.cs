namespace ProgressTrackerBot.Host.Notifications.Data;

internal interface INotificationsCleanerRepository
{
    public Task<int> RemoveOutdatedNotifications(DateTime boundaryDateTime, int limit = 100);
}