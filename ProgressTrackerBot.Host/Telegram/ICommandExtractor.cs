namespace ProgressTrackerBot.Host.Telegram;

internal interface ICommandExtractor
{
    public string Extract(string? text);   
}