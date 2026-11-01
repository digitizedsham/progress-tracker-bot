using Dapper;
using ProgressTrackerBot.Host.Common.DbConnectionFactory;
using ProgressTrackerBot.Host.Features.Start.Data.Models;

namespace ProgressTrackerBot.Host.Features.Start.Data;

internal sealed class RegisterUserRepository(IDbConnectionFactory dbConnectionFactory) : IRegisterUserRepository
{
    public async Task AddOrUpdate(AddUserModel model)
    {
        const string sql = $@"
                           INSERT INTO public.users (
                               id,
                               username
                           )
                           VALUES (
                               @{nameof(AddUserModel.Id)},
                               @{nameof(AddUserModel.Username)}
                           )
                           ON CONFLICT (id)
                           DO UPDATE SET
                               username = EXCLUDED.username
                           ";

        await using var connection = dbConnectionFactory.Create();

        await connection.ExecuteAsync(sql, model);
    }
}