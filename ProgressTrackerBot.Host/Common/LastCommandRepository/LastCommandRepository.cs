using Dapper;
using ProgressTrackerBot.Host.Common.DbConnectionFactory;
using ProgressTrackerBot.Host.Common.LastCommandRepository.Models;

namespace ProgressTrackerBot.Host.Common.LastCommandRepository;

internal class LastCommandRepository(IDbConnectionFactory dbConnectionFactory) : ILastCommandRepository
{
    public async Task AddOrUpdate(AddLastCommandModel model)
    {
        const string sql = $@"
                           INSERT INTO public.user_last_commands (
                               user_id,
                               command_name
                           )
                           VALUES (
                               @{nameof(AddLastCommandModel.UserId)},
                               @{nameof(AddLastCommandModel.Command)}
                           )
                           ON CONFLICT (user_id)
                           DO UPDATE SET
                               command_name = EXCLUDED.command_name,
                               updated_at = NOW();
                           ";
        
        await using var connection = dbConnectionFactory.Create();

        await connection.ExecuteAsync(sql, model);
    }
}