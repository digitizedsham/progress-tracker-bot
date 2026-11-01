namespace ProgressTrackerBot.Host.Entities.Track;

internal class Track
{
    public long Id { get; set; }
    
    public long UserId { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public short WeekdaysMask { get; set; }
    
    public bool SoundOn { get; set; }

    public short ReminderHour { get; set; }
}