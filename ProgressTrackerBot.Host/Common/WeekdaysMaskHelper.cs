namespace ProgressTrackerBot.Host.Common;

internal static class WeekdaysMaskHelper
{
    public static short WeekdayMask(this DayOfWeek dayOfWeek) => dayOfWeek switch
    {
        DayOfWeek.Monday => 1,
        DayOfWeek.Tuesday => 2,
        DayOfWeek.Wednesday => 4,
        DayOfWeek.Thursday => 8,
        DayOfWeek.Friday => 16,
        DayOfWeek.Saturday => 32,
        DayOfWeek.Sunday => 64,
        _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek))
    };
}