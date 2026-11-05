namespace ProgressTrackerBot.Host.Features.AddTrack.Data.Models;

internal class AddTrackNameModel
{
    public long UserId { get; set; }

    public string Name { get; set; } =  string.Empty;
}