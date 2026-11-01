namespace ProgressTrackerBot.Host.Features.EditTrack.Data.Models;

internal class EditTrackSettingsModel
{
    public long Id { get; set; }

    public short WeekdaysMask { get; set; }
    
    public bool SoundOn { get; set; }

    public short ReminderHour { get; set; }
}