using ProgressTrackerBot.Host.Features.Start.Data.Models;

namespace ProgressTrackerBot.Host.Features.Start.Data;

internal interface IRegisterUserRepository
{
    public Task AddOrUpdate(AddUserModel model);
}