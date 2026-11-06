using ProgressTrackerBot.Host.Notifications.Data;

namespace ProgressTrackerBot.Host.Notifications.Infrastructure;

internal static class ServiceCollectionExtensions
{
    public static void AddNotifications(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<INotificationsSchedulerRepository, NotificationsSchedulerRepository>();
        services.AddSingleton<INotificationsCleanerRepository, NotificationsCleanerRepository>();
        services.AddSingleton<INotificationsSenderRepository, NotificationsSenderRepository>();
        services.AddHostedService<NotificationsScheduler>();
        services.AddHostedService<NotificationsSender>();
        services.AddHostedService<NotificationsCleaner>();
    }
}