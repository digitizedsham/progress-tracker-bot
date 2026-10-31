using Npgsql;

namespace ProgressTrackerBot.Host.Common.DbConnectionFactory;

internal class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public NpgsqlConnection Create()
    {
        return new NpgsqlConnection(connectionString);
    }
}