using ProgressTrackerBot.Host.Entities.Tracks;
using ProgressTrackerBot.Host.Features.EditTrack.Data.Models;

namespace ProgressTrackerBot.Host.Features.EditTrack.Data;

internal interface IEditTrackRepository
{
    public Task<Track?> EditTrackSettings(EditTrackSettingsModel model);
    
    public Task<Track?> GetTrack(long trackId);
}