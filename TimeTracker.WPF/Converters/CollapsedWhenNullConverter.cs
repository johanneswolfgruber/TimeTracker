namespace TimeTracker.WPF.Converters;

public class CollapsedWhenNullConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value is not null) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is Visibility visibility && visibility == Visibility.Visible;
    }
}
