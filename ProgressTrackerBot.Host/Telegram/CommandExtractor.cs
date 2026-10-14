namespace ProgressTrackerBot.Host.Telegram;

internal class CommandExtractor : ICommandExtractor
{
    private const string CommandSeparator = " ";

    public string Extract(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }
        
        if (!text.StartsWith('/'))
        {
            return string.Empty;
        }
        
        var commandSplit = text.Split(CommandSeparator);

        if (commandSplit.Length > 0)
        {
            return commandSplit[0];
        }

        return string.Empty;
    }
}