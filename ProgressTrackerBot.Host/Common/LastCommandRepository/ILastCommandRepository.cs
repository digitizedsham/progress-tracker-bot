using ProgressTrackerBot.Host.Common.LastCommandRepository.Models;
using ProgressTrackerBot.Host.Entities.LastCommand;

namespace ProgressTrackerBot.Host.Common.LastCommandRepository;

internal interface ILastCommandRepository
{
    public Task AddOrUpdate(AddLastCommandModel model);
    
    public Task<LastCommand?> GetLastCommand(long userId);
}