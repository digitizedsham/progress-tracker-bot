using Npgsql;

namespace ProgressTrackerBot.Host.Common.DbConnectionFactory;

internal interface IDbConnectionFactory
{
    NpgsqlConnection Create();
}