namespace ProgressTrackerBot.Host.Notifications.Data.Models;

internal class NotificationToSendModel
{
    public long Id { get; set; }
    
    public long TrackId { get; set; }

    public long UserId { get; set; }

    public DateTimeOffset ScheduledAt { get; set; }
    
    public short Attempts { get; set; } 
}