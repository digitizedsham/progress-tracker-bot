using System.Diagnostics;
using ProgressTrackerBot.Host.Common;
using ProgressTrackerBot.Host.Notifications.Data;
using ProgressTrackerBot.Host.Notifications.Data.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProgressTrackerBot.Host.Notifications;

internal class NotificationsSender(
    ITelegramBotClient bot,
    INotificationsSenderRepository notificationsSenderRepository,
    ILogger<NotificationsSender> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    { 
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                // TODO: переделать все более вдумчиво, если появится хоть какая-то нагрузка (более 100 уведомлений в час).
                var sentNotificationsCount = await SendNotifications(stoppingToken);
                
                stopwatch.Stop();
                logger.LogInformation("Рассылка {SentNotificationsCount} уведомлений заняла {ElapsedMilliseconds} миллисекунд.",
                    sentNotificationsCount,
                    stopwatch.ElapsedMilliseconds);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "При рассылке уведомлений произошло исключение.");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task<int> SendNotifications(CancellationToken cancellationToken)
    {
        var notificationsToSend = (await notificationsSenderRepository.GetNotificationsToSend()).ToArray();

        foreach (var notification in notificationsToSend)
        {
            await SendNotification(notification, cancellationToken);
            await Task.Delay(1000, cancellationToken);
        }

        return notificationsToSend.Length;
    }

    private async Task SendNotification(NotificationToSendModel notification, CancellationToken cancellationToken)
    {
        try
        {
            await bot.SendMessage(
                chatId: notification.UserId,
                text: GetMessageText(notification),
                parseMode: ParseMode.Html,
                disableNotification: true,
                replyMarkup: GetMenuKeyboard(),
                cancellationToken: cancellationToken);
            
            await notificationsSenderRepository.MarkNotificationAsSent(notification.Id);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex, 
                "Не удалось отправить уведомление Id:{NotificationId} по цели TrackId:{TrackId} для пользователя UserId:{UserId}.",
                notification.Id,
                notification.TrackId,
                notification.UserId);

            if (notification.Attempts > 2)
            {
                await notificationsSenderRepository.MarkNotificationAsFailed(notification.Id, ex.Message);
            }
        }
    }

    private static string GetMessageText(NotificationToSendModel notification)
    {
        return $"Вы выполнили свою цель \"{notification.Id}\" {notification.ScheduledAt}?";
    }
    
    private static InlineKeyboardMarkup GetMenuKeyboard()
    {
        return new InlineKeyboardMarkup([
            [
                InlineKeyboardButton.WithCallbackData(
                    text: "Выполнил",
                    callbackData: $"{CommandNameConstants.SetTrackResultCommandName} {true}"),
                InlineKeyboardButton.WithCallbackData(
                    text: "Не выполнил",
                    callbackData: $"{CommandNameConstants.SetTrackResultCommandName} {false}")
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    text: $"{CommandTitleConstants.MenuCommandTitle}",
                    callbackData: CommandNameConstants.NewMessageMenuCommandName)
            ]
        ]);
    }
}