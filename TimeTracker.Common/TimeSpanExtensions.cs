namespace TimeTracker.Common;

public static class TimeSpanExtensions
{
    public static string ToDurationFormatStringFull(this TimeSpan duration)
    {
        return $"{(int)duration.TotalHours:d2}h " +
               $"{duration.Minutes:d2}min " +
               $"{duration.Seconds:d2}sec";
    }

    public static string ToDurationFormatStringWithoutSeconds(this TimeSpan duration)
    {
        return $"{(int)duration.TotalHours:d2}h " +
               $"{duration.Minutes:d2}min";
    }

    public static string ToDurationFormatStringSingleFull(this TimeSpan duration)
    {
        return $"{duration.Hours:d2}h " +
               $"{duration.Minutes:d2}min " +
               $"{duration.Seconds:d2}sec";
    }

    public static string ToDurationFormatStringSingleWithoutSeconds(this TimeSpan duration)
    {
        return $"{duration.Hours:d2}h " +
               $"{duration.Minutes:d2}min";
    }
}