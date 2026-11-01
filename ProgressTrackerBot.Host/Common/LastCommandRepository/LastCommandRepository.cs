using Dapper;
using ProgressTrackerBot.Host.Common.DbConnectionFactory;
using ProgressTrackerBot.Host.Common.LastCommandRepository.Models;
using ProgressTrackerBot.Host.Entities.LastCommand;

namespace ProgressTrackerBot.Host.Common.LastCommandRepository;

internal class LastCommandRepository(IDbConnectionFactory dbConnectionFactory) : ILastCommandRepository
{
    public async Task AddOrUpdate(AddLastCommandModel model)
    {
        const string sql = $@"
                           INSERT INTO public.user_last_commands (
                               user_id,
                               command_name,
                               arguments                                   
                           )
                           VALUES (
                               @{nameof(AddLastCommandModel.UserId)},
                               @{nameof(AddLastCommandModel.Command)},
                               @{nameof(AddLastCommandModel.Arguments)}
                           )
                           ON CONFLICT (user_id)
                           DO UPDATE SET
                               command_name = EXCLUDED.command_name,
                               arguments = EXCLUDED.arguments,
                               updated_at = NOW();
                           ";
        
        await using var connection = dbConnectionFactory.Create();

        await connection.ExecuteAsync(sql, model);
    }

    public async Task<LastCommand?> GetLastCommand(long userId)
    {
        const string sql = $@"
                           SELECT
                               user_id AS {nameof(LastCommand.UserId)},
                               command_name AS {nameof(LastCommand.Command)},
                               arguments AS {nameof(LastCommand.Arguments)}
                           FROM public.user_last_commands
                           WHERE user_id = @UserId;
                           ";

        await using var connection = dbConnectionFactory.Create();

        return await connection.QuerySingleOrDefaultAsync<LastCommand>(
            sql,
            new { UserId = userId }
        );
    }
}