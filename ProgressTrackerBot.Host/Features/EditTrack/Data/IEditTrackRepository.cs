using ProgressTrackerBot.Host.Entities.Track;
using ProgressTrackerBot.Host.Features.EditTrack.Data.Models;

namespace ProgressTrackerBot.Host.Features.EditTrack.Data;

internal interface IEditTrackRepository
{
    public Task<Track?> EditTrackSettings(EditTrackSettingsModel model);
}