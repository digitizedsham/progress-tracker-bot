using Dapper;
using ProgressTrackerBot.Host.Common.DbConnectionFactory;
using ProgressTrackerBot.Host.Entities.Users;

namespace ProgressTrackerBot.Host.Features.Start.Data;

internal sealed class RegisterUserRepository(IDbConnectionFactory dbConnectionFactory) : IRegisterUserRepository
{
    public async Task AddOrUpdate(long telegramId, string? username)
    {
        const string sql = $@"
                           INSERT INTO users (
                               telegram_id,
                               username
                           )
                           VALUES (
                               @{nameof(User.TelegramId)},
                               @{nameof(User.Username)}
                           )
                           ON CONFLICT (telegram_id)
                           DO UPDATE SET
                               username = EXCLUDED.username
                           ";

        await using var connection = dbConnectionFactory.Create();

        await connection.ExecuteAsync(
            sql,
            new
            {
                TelegramId = telegramId,
                Username = username
            });
    }
}