using ProgressTrackerBot.Host.Common.LastCommandRepository.Models;

namespace ProgressTrackerBot.Host.Common.LastCommandRepository;

internal interface ILastCommandRepository
{
    public Task AddOrUpdate(AddLastCommandModel model);
}