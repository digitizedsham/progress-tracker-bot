namespace ProgressTrackerBot.Host.Entities.Notifications;

internal class Notification
{
    public long Id { get; set; }
    
    public long TrackId { get; set; }
    
    public long UserId { get; set; }
    
    public DateTimeOffset ScheduledAt { get; set; }
    
    public string Status { get; set; } = string.Empty;
    
    public short Attempts { get; set; } 
    
    public DateTimeOffset NextAttemptAt { get; set; }
    
    public DateTimeOffset LockedUntil { get; set; }
    
    public DateTimeOffset SentAt { get; set; }
    
    public string LastError { get; set; } = string.Empty;
    
    public DateTimeOffset CreatedAt { get; set; }
}