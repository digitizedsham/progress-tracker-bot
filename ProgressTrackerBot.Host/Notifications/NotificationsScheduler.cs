using System.Diagnostics;
using ProgressTrackerBot.Host.Common;
using ProgressTrackerBot.Host.Notifications.Data;

namespace ProgressTrackerBot.Host.Notifications;

internal class NotificationsScheduler(
    INotificationsSchedulerRepository notificationsSchedulerRepository,
    ILogger<NotificationsScheduler> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                
                await ScheduleTracks();
                
                stopwatch.Stop();
                logger.LogInformation("Планировщик отработал за {ElapsedMilliseconds} миллисекунд.",
                    stopwatch.ElapsedMilliseconds);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "В работе планировщика произошло исключение.");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task ScheduleTracks()
    {
        var currentUtcDateTime = DateTime.UtcNow;
        var currentMoscowDateTime = currentUtcDateTime.AddHours(3);
        var scheduledAt = 
            new DateTime(currentUtcDateTime.Year, currentUtcDateTime.Month, currentUtcDateTime.Day, currentUtcDateTime.Hour, 0, 0, 0);
        var weekdayMask = currentMoscowDateTime.DayOfWeek.WeekdayMask();
        var reminderHour = (short)currentMoscowDateTime.Hour;
        var unscheduledTracks = 
            (await notificationsSchedulerRepository.GetUnscheduledTrackNotifications(scheduledAt, weekdayMask, reminderHour)).ToArray();

        if (unscheduledTracks.Length == 0)
        {
            return;
        }

        await notificationsSchedulerRepository.ScheduleTrackNotifications(
            scheduledAt, 
            unscheduledTracks.Select(track => track.Id).ToArray(),
            unscheduledTracks.Select(track => track.UserId).ToArray());
        logger.LogInformation("Было запланировано {NotificationsCount} уведомлений на отправку.", unscheduledTracks.Length);
    }
}