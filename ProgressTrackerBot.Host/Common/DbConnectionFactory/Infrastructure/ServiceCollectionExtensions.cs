namespace ProgressTrackerBot.Host.Common.DbConnectionFactory.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddDbConnectionFactory(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration
            .GetConnectionString("DefaultConnection")!;

        services.AddSingleton<IDbConnectionFactory>(
            new DbConnectionFactory(connectionString));
    }
}