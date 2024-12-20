﻿namespace TimeTracker.WPF.Converters;

public class CollapsedWhenTrueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value is bool boolean && !boolean) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is Visibility visibility && visibility != Visibility.Visible;
    }
}
