using ProgressTrackerBot.Host.Entities.Tracks;
using ProgressTrackerBot.Host.Features.AddTrack.Data.Models;

namespace ProgressTrackerBot.Host.Features.AddTrack.Data;

internal interface IAddTrackRepository
{
    public Task<Track> AddTrack(AddTrackNameModel model);
}