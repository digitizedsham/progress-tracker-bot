namespace ProgressTrackerBot.Host.Telegram.UpdateHandlers;

internal class UpdateHandleResult
{
    public long UserId { get; set; }
    
    public string Command { get; set; } = string.Empty;
    
    public string Arguments { get; set; } = string.Empty;
}