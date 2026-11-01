namespace ProgressTrackerBot.Host.Entities.LastCommand;

internal class LastCommand
{
    public long UserId { get; set; }
    
    public string Command { get; set; } = string.Empty;
    
    // TODO: возможно это не нужно и можно удалить. 
    public string Arguments { get; set; } = string.Empty;
}