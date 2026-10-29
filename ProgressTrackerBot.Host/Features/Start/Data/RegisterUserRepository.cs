namespace ProgressTrackerBot.Host.Features.Start.Data;

internal sealed class RegisterUserRepository : IRegisterUserRepository
{
    public Task AddOrUpdate(long telegramId, string? firstName, string? lastName, string? username,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException(); 
    }
}