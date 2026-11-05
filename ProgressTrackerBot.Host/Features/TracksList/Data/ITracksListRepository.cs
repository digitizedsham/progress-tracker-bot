using ProgressTrackerBot.Host.Entities.Track;

namespace ProgressTrackerBot.Host.Features.TracksList.Data;

internal interface ITracksListRepository
{
    public Task<int> GetTracksCount(long userId);
    
    public Task<IEnumerable<Track>> GetTracksListByOffset(long userId, int offset, int limit = 10);
}