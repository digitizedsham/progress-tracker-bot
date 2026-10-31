using ProgressTrackerBot.Host.Features.Start.Data;
using ProgressTrackerBot.Host.Features.Start.Handlers;
using ProgressTrackerBot.Host.Telegram;

namespace ProgressTrackerBot.Host.Features.Start.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddStartFeature(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRegisterUserRepository, RegisterUserRepository>();
        services.AddScoped<IMessageCommandHandler, StartMessageCommandHandler>();
    }
}