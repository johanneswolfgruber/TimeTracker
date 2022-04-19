namespace TimeTracker.Common;

public static class TimeSpanExtensions
{
    public static string ToDurationFormatString(this TimeSpan duration)
    {
        return $"{(int)duration.TotalHours:d2}:" +
               $"{duration.Minutes:d2}:" +
               $"{duration.Seconds:d2}";
    }
}