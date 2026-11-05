using System.Text;
using ProgressTrackerBot.Host.Common;
using ProgressTrackerBot.Host.Entities.Track;
using ProgressTrackerBot.Host.Features.TracksList.Data;
using ProgressTrackerBot.Host.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProgressTrackerBot.Host.Features.TracksList.Handlers;

internal class TracksListCallbackQueryCommandHandler(    
    ITelegramBotClient bot,
    ITracksListRepository tracksListRepository) : ICallbackQueryCommandHandler
{
    public string CommandName => CommandNameConstants.TracksListCommandName;

    public async Task Handle(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var tracksCount = await tracksListRepository.GetTracksCount(callbackQuery.Message!.Chat.Id);
        
        if (tracksCount == 0)
        {
            await bot.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: GetNoTracksMessageText(),
                parseMode: ParseMode.Html,
                replyMarkup: GetNoTracksKeyboard(), 
                cancellationToken: cancellationToken);
            
            return;
        }
        
        var arguments = callbackQuery.Data!.Split(" ").Skip(1).ToArray();
        var offset = int.Parse(arguments[0]);
        
        var tracks = (await tracksListRepository.GetTracksListByOffset(callbackQuery.Message.Chat.Id, offset)).ToArray();
        
        await bot.EditMessageText(
            chatId: callbackQuery.Message!.Chat.Id,
            messageId: callbackQuery.Message.MessageId,
            text: GetMessageText(tracksCount, offset, tracks),
            parseMode: ParseMode.Html,
            replyMarkup: GetKeyboard(tracksCount, offset, tracks), 
            cancellationToken: cancellationToken);
    }
    
    private static string GetNoTracksMessageText()
    {
        return "У вас пока нет ни одной цели";
    }

    private static InlineKeyboardMarkup GetNoTracksKeyboard()
    {
        return new InlineKeyboardMarkup([
            [
                InlineKeyboardButton.WithCallbackData(
                    text: CommandTitleConstants.AddTrackCommandTitle,
                    callbackData: CommandNameConstants.AddTrackCommandName)
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    text: $"« {CommandTitleConstants.MenuCommandTitle}",
                    callbackData: CommandNameConstants.MenuCommandName)
            ]
        ]);
    }
    
    private static string GetMessageText(int tracksCount, int offset, ICollection<Track> tracks)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"<b>{CommandTitleConstants.TracksListCommandTitle}</b>\n");
        stringBuilder.AppendLine($"Всего целей: {tracksCount}");
        stringBuilder.AppendLine("Нажмите кнопку, соответствующую номеру цели, чтобы отредактировать или удалить ее.\n");
        for (var i = 0; i < tracks.Count; i++)
        {
            var track = tracks.ElementAt(i);
            stringBuilder.AppendLine($"{offset + i + 1}) {track.Name}");
        }
        return stringBuilder.ToString();
    }

    private InlineKeyboardMarkup GetKeyboard(int tracksCount, int offset, ICollection<Track> tracks)
    {
        var buttons = new List<List<InlineKeyboardButton>>();

        var buttonRow = new List<InlineKeyboardButton>();
        for (var i = 0; i < tracks.Count; i++)
        {
            buttonRow.Add(
                InlineKeyboardButton.WithCallbackData(
                    text: $"{offset + i + 1}",
                    callbackData: $"{CommandNameConstants.EditTrackCommandName} {tracks.ElementAt(i).Id}")
            );

            if (buttonRow.Count == 5)
            {
                buttons.Add(buttonRow);
                buttonRow = new List<InlineKeyboardButton>();
            }
        }
        
        buttons.Add(buttonRow);
        
        buttons.Add(GetArrowButtons(tracksCount, offset));

        buttons.Add([
            InlineKeyboardButton.WithCallbackData(
                text: $"« {CommandTitleConstants.MenuCommandTitle}",
                callbackData: CommandNameConstants.MenuCommandName)
        ]);
        return buttons;
    }

    private List<InlineKeyboardButton> GetArrowButtons(int tracksCount, int offset)
    {
        var result = new List<InlineKeyboardButton>();

        const int numberOfTracksToShow = 10;
        if (offset >= numberOfTracksToShow)
        {
            result.Add(InlineKeyboardButton.WithCallbackData(
                text: $"◀️ {offset + 1 - 10} - {offset}",
                callbackData: $"{CommandNameConstants.TracksListCommandName} {offset - numberOfTracksToShow}"));
        }

        if (tracksCount > offset + numberOfTracksToShow)
        {
            var firstPart = offset + numberOfTracksToShow + 1;
            var secondPart = offset + 2 * numberOfTracksToShow < tracksCount
                ? offset + 2 * numberOfTracksToShow
                : tracksCount;
            var buttonText = firstPart == secondPart
                ? $"{firstPart} ▶️"
                : $"{offset + numberOfTracksToShow + 1} - {secondPart} ▶️";
            result.Add(InlineKeyboardButton.WithCallbackData(
                text: buttonText,
                callbackData: $"{CommandNameConstants.TracksListCommandName} {offset + numberOfTracksToShow}"));
        }
        
        return result;
    }
}