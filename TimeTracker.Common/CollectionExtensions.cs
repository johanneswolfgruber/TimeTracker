namespace TimeTracker.Common;

public static class CollectionExtensions
{
    public static TimeSpan Sum(this IEnumerable<TimeSpan> collection)
    {
        return collection.Aggregate(TimeSpan.Zero, (current, v) => current + v);
    }
}