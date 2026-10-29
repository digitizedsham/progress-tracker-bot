namespace ProgressTrackerBot.Host.Features.Start.Data;

internal interface IRegisterUserRepository
{
    public Task AddOrUpdate(long telegramId, string? firstName, string? lastName, string? username, CancellationToken cancellationToken = default);
}