using ProgressTrackerBot.Host.Common.LastCommandRepository;
using ProgressTrackerBot.Host.Common.LastCommandRepository.Models;
using ProgressTrackerBot.Host.Telegram.UpdateHandlers;
using Telegram.Bot.Types;

namespace ProgressTrackerBot.Host.Telegram;

internal sealed class UpdateDispatcher(
    IServiceScopeFactory scopeFactory,
    ILogger<UpdateDispatcher> logger)
{
    public async Task Handle(
        Update update,
        CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();

            var updateHandler = scope.ServiceProvider
                .GetServices<IUpdateHandler>()
                .SingleOrDefault(h => h.UpdateType == update.Type);

            if (updateHandler != null)
            {
                var lastCommand = await updateHandler.Handle(update, cancellationToken);
                if (lastCommand == null)
                {
                    return;
                }
                
                var lastCommandRepository = scope.ServiceProvider.GetRequiredService<ILastCommandRepository>();
                await lastCommandRepository.AddOrUpdate(
                    new AddLastCommandModel
                    {
                        UserId = lastCommand.UserId,
                        Command = lastCommand.Command,
                    });

                return;
            }

            logger.LogError("Тип события \"{UpdateType}\" не поддерживается.", update.Type.ToString());
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка обработки события из телеграма.");
        }
    }
}