namespace ProgressTrackerBot.Host.Common.LastCommandRepository.Models;

internal class AddLastCommandModel
{
    public long UserId { get; set; }
    
    public string Command { get; set; } = string.Empty;
}