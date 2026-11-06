using ProgressTrackerBot.Host.Common.LastCommandRepository.Models;
using ProgressTrackerBot.Host.Entities.LastCommands;

namespace ProgressTrackerBot.Host.Common.LastCommandRepository;

internal interface ILastCommandRepository
{
    public Task AddOrUpdate(AddLastCommandModel model);
    
    public Task<LastCommand?> GetLastCommand(long userId);
}