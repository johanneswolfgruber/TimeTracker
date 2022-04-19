namespace TimeTracker.WPF.Converters;

public class TotalDurationConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is not Dictionary<string, TimeSpan> groupDurations || values[1] is not string group)
        {
            return null!;
        }

        if (!groupDurations.TryGetValue(group, out var timeSpan))
        {
            return null!;
        }

        return timeSpan.ToDurationFormatString();
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

