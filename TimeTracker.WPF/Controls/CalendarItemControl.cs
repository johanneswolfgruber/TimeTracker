namespace TimeTracker.WPF.Controls;

public class CalendarItemControl : UserControl
{
    static CalendarItemControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(CalendarItemControl),
            new FrameworkPropertyMetadata(typeof(CalendarItemControl))
        );
    }

    public string DisplayDuration
    {
        get { return (string)GetValue(DisplayDurationProperty); }
        set { SetValue(DisplayDurationProperty, value); }
    }

    public static readonly DependencyProperty DisplayDurationProperty = DependencyProperty.Register(
        nameof(DisplayDuration),
        typeof(string),
        typeof(CalendarItemControl),
        new PropertyMetadata("0h 0min 0sec")
    );

    public string ActivityName
    {
        get { return (string)GetValue(ActivityNameProperty); }
        set { SetValue(ActivityNameProperty, value); }
    }

    public static readonly DependencyProperty ActivityNameProperty = DependencyProperty.Register(
        nameof(ActivityName),
        typeof(string),
        typeof(CalendarItemControl),
        new PropertyMetadata("ActivityName")
    );

    public bool IsReadOnly
    {
        get { return (bool)GetValue(IsReadOnlyProperty); }
        set { SetValue(IsReadOnlyProperty, value); }
    }

    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
        nameof(IsReadOnly),
        typeof(bool),
        typeof(CalendarItemControl),
        new PropertyMetadata(true)
    );

    public ICommand StopCommand
    {
        get { return (ICommand)GetValue(StopCommandProperty); }
        set { SetValue(StopCommandProperty, value); }
    }

    public static readonly DependencyProperty StopCommandProperty = DependencyProperty.Register(
        nameof(StopCommand),
        typeof(ICommand),
        typeof(CalendarItemControl),
        new PropertyMetadata(default(ICommand))
    );

    public bool IsSelected
    {
        get { return (bool)GetValue(IsSelectedProperty); }
        set { SetValue(IsSelectedProperty, value); }
    }

    public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
        nameof(IsSelected),
        typeof(bool),
        typeof(CalendarItemControl),
        new PropertyMetadata(false)
    );

    public CalendarItemViewModel CalendarItem
    {
        get { return (CalendarItemViewModel)GetValue(CalendarItemProperty); }
        set { SetValue(CalendarItemProperty, value); }
    }

    public static readonly DependencyProperty CalendarItemProperty = DependencyProperty.Register(
        nameof(CalendarItem),
        typeof(CalendarItemViewModel),
        typeof(CalendarItemControl),
        new PropertyMetadata(default(CalendarItemViewModel))
    );
}
