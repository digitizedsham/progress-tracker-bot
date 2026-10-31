namespace ProgressTrackerBot.Host.Entities.Users;

internal class User
{
    public long Id { get; set; }
    
    public long TelegramId { get; set; }
    
    public string? Username { get; set; }
    
    public DateTimeOffset? CreatedAt { get; set; }
    
    public DateTimeOffset? PremiumUntil { get; set; }
}