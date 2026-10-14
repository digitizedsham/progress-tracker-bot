namespace ProgressTrackerBot.Host.Telegram.CommandExtractor;

internal interface ICommandExtractor
{
    public string Extract(string? text);   
}