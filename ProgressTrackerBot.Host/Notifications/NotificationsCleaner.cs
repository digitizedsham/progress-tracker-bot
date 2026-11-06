using System.Diagnostics;
using ProgressTrackerBot.Host.Notifications.Data;

namespace ProgressTrackerBot.Host.Notifications;

internal class NotificationsCleaner(
    INotificationsCleanerRepository notificationsCleanerRepository,
    ILogger<NotificationsCleaner> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                
                var removedNotificationsCount = await CleanOutdatedNotifications();
                
                stopwatch.Stop();
                logger.LogInformation("Удаление устаревших уведомлений отработало за {ElapsedMilliseconds} миллисекунд. Было удалено {RemovedNotificationsCount} записей.",
                    stopwatch.ElapsedMilliseconds,
                    removedNotificationsCount);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Во время очистки устаревших уведомлений произошло исключение.");
            }

            await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken); 
        }
    }

    private Task<int> CleanOutdatedNotifications()
    {
        var boundaryDate = DateTime.UtcNow.AddDays(-30);
        return notificationsCleanerRepository.RemoveOutdatedNotifications(boundaryDate);
    }
}