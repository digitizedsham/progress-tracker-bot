namespace ProgressTrackerBot.Host.Common.LastCommandRepository.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddLastCommandRepository(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILastCommandRepository, LastCommandRepository>();
    }
}