using ProgressTrackerBot.Host.Entities.Tracks;

namespace ProgressTrackerBot.Host.Notifications.Data;

internal interface INotificationsSchedulerRepository
{
    public Task<IEnumerable<Track>> GetUnscheduledTrackNotifications(DateTime scheduledAt, short weekdayMask, short reminderHour, short limit = 100);

    public Task ScheduleTrackNotifications(DateTime scheduledAt, long[] trackIds, long[] userIds);
}