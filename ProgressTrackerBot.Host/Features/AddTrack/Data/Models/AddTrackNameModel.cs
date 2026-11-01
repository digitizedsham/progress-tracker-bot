namespace ProgressTrackerBot.Host.Features.AddTrack.Data.Models;

public class AddTrackNameModel
{
    public long UserId { get; set; }

    public string Name { get; set; } =  string.Empty;
}