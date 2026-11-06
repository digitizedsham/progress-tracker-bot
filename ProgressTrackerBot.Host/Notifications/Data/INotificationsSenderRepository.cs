using ProgressTrackerBot.Host.Notifications.Data.Models;

namespace ProgressTrackerBot.Host.Notifications.Data;

internal interface INotificationsSenderRepository
{
    public Task<IEnumerable<NotificationToSendModel>> GetNotificationsToSend(short limit = 100);
    
    public Task MarkNotificationAsSent(long notificationId);
    
    public Task MarkNotificationAsFailed(long notificationId, string errorMessage);
    
}