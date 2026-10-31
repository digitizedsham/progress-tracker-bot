using ProgressTrackerBot.Host.Common.DbConnectionFactory.Infrastructure;

namespace ProgressTrackerBot.Host.Common.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddCommon(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbConnectionFactory(configuration);
    }
}