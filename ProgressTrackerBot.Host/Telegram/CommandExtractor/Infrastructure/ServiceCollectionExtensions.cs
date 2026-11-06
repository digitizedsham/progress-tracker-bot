namespace ProgressTrackerBot.Host.Telegram.CommandExtractor.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddCommandExtractor(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICommandExtractor, CommandExtractor>();
    }
}