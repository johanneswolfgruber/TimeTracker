using System.Collections.Specialized;
using System.Windows.Input;
using TimeTracker.WPF.ViewModels;

namespace TimeTracker.WPF.Controls;

[TemplatePart(Name = MondayCanvas, Type = typeof(Canvas))]
[TemplatePart(Name = TuesdayCanvas, Type = typeof(Canvas))]
[TemplatePart(Name = WednesdayCanvas, Type = typeof(Canvas))]
[TemplatePart(Name = ThursdayCanvas, Type = typeof(Canvas))]
[TemplatePart(Name = FridayCanvas, Type = typeof(Canvas))]
[TemplatePart(Name = SaturdayCanvas, Type = typeof(Canvas))]
[TemplatePart(Name = SundayCanvas, Type = typeof(Canvas))]
[TemplatePart(Name = Month, Type = typeof(TextBlock))]
[TemplatePart(Name = CalendarWeek, Type = typeof(TextBlock))]
[TemplatePart(Name = MondayHeader, Type = typeof(TextBlock))]
[TemplatePart(Name = TuesdayHeader, Type = typeof(TextBlock))]
[TemplatePart(Name = WednesdayHeader, Type = typeof(TextBlock))]
[TemplatePart(Name = ThursdayHeader, Type = typeof(TextBlock))]
[TemplatePart(Name = FridayHeader, Type = typeof(TextBlock))]
[TemplatePart(Name = SaturdayHeader, Type = typeof(TextBlock))]
[TemplatePart(Name = SundayHeader, Type = typeof(TextBlock))]
[TemplatePart(Name = MondayTotalTime, Type = typeof(TextBlock))]
[TemplatePart(Name = TuesdayTotalTime, Type = typeof(TextBlock))]
[TemplatePart(Name = WednesdayTotalTime, Type = typeof(TextBlock))]
[TemplatePart(Name = ThursdayTotalTime, Type = typeof(TextBlock))]
[TemplatePart(Name = FridayTotalTime, Type = typeof(TextBlock))]
[TemplatePart(Name = SaturdayTotalTime, Type = typeof(TextBlock))]
[TemplatePart(Name = SundayTotalTime, Type = typeof(TextBlock))]
[TemplatePart(Name = MondayBillableTime, Type = typeof(TextBlock))]
[TemplatePart(Name = TuesdayBillableTime, Type = typeof(TextBlock))]
[TemplatePart(Name = WednesdayBillableTime, Type = typeof(TextBlock))]
[TemplatePart(Name = ThursdayBillableTime, Type = typeof(TextBlock))]
[TemplatePart(Name = FridayBillableTime, Type = typeof(TextBlock))]
[TemplatePart(Name = SaturdayBillableTime, Type = typeof(TextBlock))]
[TemplatePart(Name = SundayBillableTime, Type = typeof(TextBlock))]
[TemplatePart(Name = TodayButton, Type = typeof(Button))]
[TemplatePart(Name = PreviousWeekButton, Type = typeof(Button))]
[TemplatePart(Name = NextWeekButton, Type = typeof(Button))]
[TemplatePart(Name = ExportButton, Type = typeof(Button))]
[TemplatePart(Name = WeekTotalTime, Type = typeof(TextBlock))]
[TemplatePart(Name = WeekDaysScrollViewer, Type = typeof(ScrollViewer))]
[TemplatePart(Name = CalendarScrollViewer, Type = typeof(ScrollViewer))]
[TemplatePart(Name = CalendarWeekDaysGrid, Type = typeof(Grid))]
[TemplatePart(Name = CalendarGrid, Type = typeof(Grid))]
public class CalendarControl : Control
{
    private const string MondayCanvas = "PART_MondayCanvas";
    private const string TuesdayCanvas = "PART_TuesdayCanvas";
    private const string WednesdayCanvas = "PART_WednesdayCanvas";
    private const string ThursdayCanvas = "PART_ThursdayCanvas";
    private const string FridayCanvas = "PART_FridayCanvas";
    private const string SaturdayCanvas = "PART_SaturdayCanvas";
    private const string SundayCanvas = "PART_SundayCanvas";
    private const string Month = "PART_Month";
    private const string CalendarWeek = "PART_CalendarWeek";
    private const string MondayHeader = "PART_MondayHeader";
    private const string TuesdayHeader = "PART_TuesdayHeader";
    private const string WednesdayHeader = "PART_WednesdayHeader";
    private const string ThursdayHeader = "PART_ThursdayHeader";
    private const string FridayHeader = "PART_FridayHeader";
    private const string SaturdayHeader = "PART_SaturdayHeader";
    private const string SundayHeader = "PART_SundayHeader";
    private const string MondayTotalTime = "PART_MondayTotalTime";
    private const string TuesdayTotalTime = "PART_TuesdayTotalTime";
    private const string WednesdayTotalTime = "PART_WednesdayTotalTime";
    private const string ThursdayTotalTime = "PART_ThursdayTotalTime";
    private const string FridayTotalTime = "PART_FridayTotalTime";
    private const string SaturdayTotalTime = "PART_SaturdayTotalTime";
    private const string SundayTotalTime = "PART_SundayTotalTime";
    private const string MondayBillableTime = "PART_MondayBillableTime";
    private const string TuesdayBillableTime = "PART_TuesdayBillableTime";
    private const string WednesdayBillableTime = "PART_WednesdayBillableTime";
    private const string ThursdayBillableTime = "PART_ThursdayBillableTime";
    private const string FridayBillableTime = "PART_FridayBillableTime";
    private const string SaturdayBillableTime = "PART_SaturdayBillableTime";
    private const string SundayBillableTime = "PART_SundayBillableTime";
    private const string TodayButton = "PART_TodayButton";
    private const string PreviousWeekButton = "PART_PreviousWeekButton";
    private const string NextWeekButton = "PART_NextWeekButton";
    private const string ExportButton = "PART_ExportButton";
    private const string WeekTotalTime = "PART_WeekTotalTime";
    private const string WeekDaysScrollViewer = "PART_WeekDaysScrollViewer";
    private const string CalendarScrollViewer = "PART_CalendarScrollViewer";
    private const string CalendarWeekDaysGrid = "PART_CalendarWeekDaysGrid";
    private const string CalendarGrid = "PART_CalendarGrid";

    private Canvas _mondayCanvas = default!;
    private Canvas _tuesdayCanvas = default!;
    private Canvas _wednesdayCanvas = default!;
    private Canvas _thursdayCanvas = default!;
    private Canvas _fridayCanvas = default!;
    private Canvas _saturdayCanvas = default!;
    private Canvas _sundayCanvas = default!;
    private TextBlock _month = default!;
    private TextBlock _calendarWeek = default!;
    private TextBlock _mondayHeader = default!;
    private TextBlock _tuesdayHeader = default!;
    private TextBlock _wednesdayHeader = default!;
    private TextBlock _thursdayHeader = default!;
    private TextBlock _fridayHeader = default!;
    private TextBlock _saturdayHeader = default!;
    private TextBlock _sundayHeader = default!;
    private TextBlock _mondayTotalTime = default!;
    private TextBlock _tuesdayTotalTime = default!;
    private TextBlock _wednesdayTotalTime = default!;
    private TextBlock _thursdayTotalTime = default!;
    private TextBlock _fridayTotalTime = default!;
    private TextBlock _saturdayTotalTime = default!;
    private TextBlock _sundayTotalTime = default!;
    private TextBlock _mondayBillableTime = default!;
    private TextBlock _tuesdayBillableTime = default!;
    private TextBlock _wednesdayBillableTime = default!;
    private TextBlock _thursdayBillableTime = default!;
    private TextBlock _fridayBillableTime = default!;
    private TextBlock _saturdayBillableTime = default!;
    private TextBlock _sundayBillableTime = default!;
    private Button _todayButton = default!;
    private Button _previousWeekButton = default!;
    private Button _nextWeekButton = default!;
    private Button _exportButton = default!;
    private TextBlock _weekTotalTime = default!;
    private ScrollViewer _weekDaysScrollViewer = default!;
    private ScrollViewer _calendarScrollViewer = default!;
    private Grid _calendarWeekDaysGrid = default!;
    private Grid _calendarGrid = default!;

    private int _currentCalendarWeek;
    private int _currentYear;
    private TimeSpan _mondayTotalDuration = TimeSpan.Zero;
    private TimeSpan _tuesdayTotalDuration = TimeSpan.Zero;
    private TimeSpan _wednesdayTotalDuration = TimeSpan.Zero;
    private TimeSpan _thursdayTotalDuration = TimeSpan.Zero;
    private TimeSpan _fridayTotalDuration = TimeSpan.Zero;
    private TimeSpan _saturdayTotalDuration = TimeSpan.Zero;
    private TimeSpan _sundayTotalDuration = TimeSpan.Zero;

    private readonly List<CalendarItemControl> _calendarItemControls = new();

    public ObservableCollection<CalendarItemViewModel> CalendarItems
    {
        get { return (ObservableCollection<CalendarItemViewModel>)GetValue(CalendarItemsProperty); }
        set { SetValue(CalendarItemsProperty, value); }
    }

    public static readonly DependencyProperty CalendarItemsProperty =
        DependencyProperty.Register(nameof(CalendarItems), typeof(ObservableCollection<CalendarItemViewModel>), typeof(CalendarControl), new PropertyMetadata(default(ObservableCollection<CalendarItemViewModel>), OnCalendarItemsPropertyChanged));

    public ObservableCollection<CalendarItemViewModel> SelectedCalendarItems
    {
        get { return (ObservableCollection<CalendarItemViewModel>)GetValue(SelectedCalendarItemsProperty); }
        set { SetValue(SelectedCalendarItemsProperty, value); }
    }

    public static readonly DependencyProperty SelectedCalendarItemsProperty =
        DependencyProperty.Register(nameof(SelectedCalendarItems), typeof(ObservableCollection<CalendarItemViewModel>), typeof(CalendarControl), new PropertyMetadata(default(ObservableCollection<CalendarItemViewModel>)));

    public ICommand ExportCommand
    {
        get { return (ICommand)GetValue(ExportCommandProperty); }
        set { SetValue(ExportCommandProperty, value); }
    }

    public static readonly DependencyProperty ExportCommandProperty =
        DependencyProperty.Register(nameof(ExportCommand), typeof(ICommand), typeof(CalendarControl), new PropertyMetadata(default(ICommand)));

    public double BillablePercentage
    {
        get { return (double)GetValue(BillablePercentageProperty); }
        set { SetValue(BillablePercentageProperty, value); }
    }

    public static readonly DependencyProperty BillablePercentageProperty =
        DependencyProperty.Register(nameof(BillablePercentage), typeof(double), typeof(CalendarControl), new PropertyMetadata(84d));

    public bool ShowOnlyWorkingDays
    {
        get { return (bool)GetValue(ShowOnlyWorkingDaysProperty); }
        set { SetValue(ShowOnlyWorkingDaysProperty, value); }
    }

    public static readonly DependencyProperty ShowOnlyWorkingDaysProperty =
        DependencyProperty.Register(nameof(ShowOnlyWorkingDays), typeof(bool), typeof(CalendarControl), new PropertyMetadata(true, OnShowOnlyWorkingDaysChanged));

    static CalendarControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarControl), new FrameworkPropertyMetadata(typeof(CalendarControl)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        InitializeTemplateParts();

        var calendar = CultureInfo.CurrentCulture.Calendar;

        _currentYear = DateTime.Today.Year;
        _currentCalendarWeek = calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        SetHeaders(FirstDateOfWeekISO8601(_currentYear, _currentCalendarWeek));

        RenderCalendarItems();

        _todayButton.Click += OnTodayButtonClick;
        _previousWeekButton.Click += OnPreviousWeekButtonClick;
        _nextWeekButton.Click += OnNextWeekButtonClick;
    }

    private void InitializeTemplateParts()
    {
        _mondayCanvas = (GetTemplateChild(MondayCanvas) as Canvas)!;
        _tuesdayCanvas = (GetTemplateChild(TuesdayCanvas) as Canvas)!;
        _wednesdayCanvas = (GetTemplateChild(WednesdayCanvas) as Canvas)!;
        _thursdayCanvas = (GetTemplateChild(ThursdayCanvas) as Canvas)!;
        _fridayCanvas = (GetTemplateChild(FridayCanvas) as Canvas)!;
        _saturdayCanvas = (GetTemplateChild(SaturdayCanvas) as Canvas)!;
        _sundayCanvas = (GetTemplateChild(SundayCanvas) as Canvas)!;
        _month = (GetTemplateChild(Month) as TextBlock)!;
        _calendarWeek = (GetTemplateChild(CalendarWeek) as TextBlock)!;
        _mondayHeader = (GetTemplateChild(MondayHeader) as TextBlock)!;
        _tuesdayHeader = (GetTemplateChild(TuesdayHeader) as TextBlock)!;
        _wednesdayHeader = (GetTemplateChild(WednesdayHeader) as TextBlock)!;
        _thursdayHeader = (GetTemplateChild(ThursdayHeader) as TextBlock)!;
        _fridayHeader = (GetTemplateChild(FridayHeader) as TextBlock)!;
        _saturdayHeader = (GetTemplateChild(SaturdayHeader) as TextBlock)!;
        _sundayHeader = (GetTemplateChild(SundayHeader) as TextBlock)!;
        _mondayTotalTime = (GetTemplateChild(MondayTotalTime) as TextBlock)!;
        _tuesdayTotalTime = (GetTemplateChild(TuesdayTotalTime) as TextBlock)!;
        _wednesdayTotalTime = (GetTemplateChild(WednesdayTotalTime) as TextBlock)!;
        _thursdayTotalTime = (GetTemplateChild(ThursdayTotalTime) as TextBlock)!;
        _fridayTotalTime = (GetTemplateChild(FridayTotalTime) as TextBlock)!;
        _saturdayTotalTime = (GetTemplateChild(SaturdayTotalTime) as TextBlock)!;
        _sundayTotalTime = (GetTemplateChild(SundayTotalTime) as TextBlock)!;
        _mondayBillableTime = (GetTemplateChild(MondayBillableTime) as TextBlock)!;
        _tuesdayBillableTime = (GetTemplateChild(TuesdayBillableTime) as TextBlock)!;
        _wednesdayBillableTime = (GetTemplateChild(WednesdayBillableTime) as TextBlock)!;
        _thursdayBillableTime = (GetTemplateChild(ThursdayBillableTime) as TextBlock)!;
        _fridayBillableTime = (GetTemplateChild(FridayBillableTime) as TextBlock)!;
        _saturdayBillableTime = (GetTemplateChild(SaturdayBillableTime) as TextBlock)!;
        _sundayBillableTime = (GetTemplateChild(SundayBillableTime) as TextBlock)!;
        _todayButton = (GetTemplateChild(TodayButton) as Button)!;
        _previousWeekButton = (GetTemplateChild(PreviousWeekButton) as Button)!;
        _nextWeekButton = (GetTemplateChild(NextWeekButton) as Button)!;
        _exportButton = (GetTemplateChild(ExportButton) as Button)!;
        _weekTotalTime = (GetTemplateChild(WeekTotalTime) as TextBlock)!;
        _weekDaysScrollViewer = (GetTemplateChild(WeekDaysScrollViewer) as ScrollViewer)!;
        _calendarScrollViewer = (GetTemplateChild(CalendarScrollViewer) as ScrollViewer)!;
        _calendarWeekDaysGrid = (GetTemplateChild(CalendarWeekDaysGrid) as Grid)!;
        _calendarGrid = (GetTemplateChild(CalendarGrid) as Grid)!;

        if (_mondayCanvas is null || _month is null || _calendarWeek is null || _mondayHeader is null || _tuesdayHeader is null || _wednesdayHeader is null ||
            _thursdayHeader is null || _fridayHeader is null || _saturdayHeader is null || _sundayHeader is null || _mondayTotalTime is null || _tuesdayTotalTime is null || 
            _wednesdayTotalTime is null || _thursdayTotalTime is null || _fridayTotalTime is null || _saturdayTotalTime is null || _sundayTotalTime is null ||
            _mondayBillableTime is null || _tuesdayBillableTime is null || _wednesdayBillableTime is null || _thursdayBillableTime is null || _fridayBillableTime is null || 
            _saturdayBillableTime is null || _sundayBillableTime is null || _todayButton is null || _previousWeekButton is null || _nextWeekButton is null ||
            _exportButton is null || _weekTotalTime is null || _weekDaysScrollViewer is null || _calendarScrollViewer is null || _calendarWeekDaysGrid is null || _calendarGrid is null)
        {
            throw new InvalidOperationException("Missing template part");
        }

        Unloaded += OnUnloaded;
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _weekDaysScrollViewer.ScrollChanged += OnScrollChanged;
        _calendarScrollViewer.ScrollChanged += OnScrollChanged;

        SetGridColumnsWidth();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (_weekDaysScrollViewer is not null)
        {
            _weekDaysScrollViewer.ScrollChanged -= OnScrollChanged;
        }

        if (_calendarScrollViewer is not null)
        {
            _calendarScrollViewer.ScrollChanged -= OnScrollChanged;
        }
    }

    private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (sender == _weekDaysScrollViewer)
        {
            _calendarScrollViewer.ScrollToHorizontalOffset(e.HorizontalOffset);
        }
        else
        {
            _weekDaysScrollViewer.ScrollToHorizontalOffset(e.HorizontalOffset);
        }
    }

    private void OnTodayButtonClick(object sender, RoutedEventArgs e)
    {
        var calendar = CultureInfo.CurrentCulture.Calendar;

        _currentYear = DateTime.Today.Year;
        _currentCalendarWeek = calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        SetHeaders(FirstDateOfWeekISO8601(_currentYear, _currentCalendarWeek));
        RenderCalendarItems();
    }

    private void OnPreviousWeekButtonClick(object sender, RoutedEventArgs e)
    {
        _currentCalendarWeek -= 1;
        if (_currentCalendarWeek < 1)
        {
            _currentYear -= 1;
            _currentCalendarWeek = 52;
        }

        SetHeaders(FirstDateOfWeekISO8601(_currentYear, _currentCalendarWeek));
        RenderCalendarItems();
    }

    private void OnNextWeekButtonClick(object sender, RoutedEventArgs e)
    {
        _currentCalendarWeek += 1;
        if (_currentCalendarWeek > 52)
        {
            _currentYear += 1;
            _currentCalendarWeek = 1;
        }


        SetHeaders(FirstDateOfWeekISO8601(_currentYear, _currentCalendarWeek));
        RenderCalendarItems();
    }

    private void SetHeaders(DateTime firstDateOfWeek)
    {
        _month.Text = $"{firstDateOfWeek:MMMM} {_currentYear}";
        _calendarWeek.Text = $"KW {_currentCalendarWeek}";
        _mondayHeader.Text = $"Mon {firstDateOfWeek.Date.Day}";
        _tuesdayHeader.Text = $"Tue {firstDateOfWeek.AddDays(1).Date.Day}";
        _wednesdayHeader.Text = $"Wed {firstDateOfWeek.AddDays(2).Date.Day}";
        _thursdayHeader.Text = $"Thu {firstDateOfWeek.AddDays(3).Date.Day}";
        _fridayHeader.Text = $"Fri {firstDateOfWeek.AddDays(4).Date.Day}";
        _saturdayHeader.Text = $"Sat {firstDateOfWeek.AddDays(5).Date.Day}";
        _sundayHeader.Text = $"Sun {firstDateOfWeek.AddDays(6).Date.Day}";
    }

    private void RenderCalendarItems()
    {
        ClearCanvases();
        ResetTimes();
        ClearCalendarItemControls();

        var firstDateOfWeek = FirstDateOfWeekISO8601(_currentYear, _currentCalendarWeek);

        var factor = BillablePercentage / 100;

        if (CalendarItems is null)
        {
            return;
        }

        var weekTotalTime = TimeSpan.Zero;

        foreach (var calendarItem in CalendarItems)
        {
            var tracking = calendarItem.Tracking;

            if (tracking.StartTime.Date < firstDateOfWeek.Date || tracking.StartTime.Date > firstDateOfWeek.AddDays(6).Date)
            {
                continue;
            }

            var dayOfWeek = tracking.StartTime.DayOfWeek;

            var canvas = GetCanvas(dayOfWeek);
            ref TimeSpan totalDuration = ref GetTotalDurationField(dayOfWeek);
            totalDuration += calendarItem.DurationTimeSpan;
            var billableDuration = totalDuration * factor;

            var totalTimeTextBlock = GetTotalTimeTextBlock(dayOfWeek);
            totalTimeTextBlock.Text = totalDuration.ToDurationFormatStringWithoutSeconds();
            var billableTimeTextBlock = GetBillableTimeTextBlock(dayOfWeek);
            billableTimeTextBlock.Text = billableDuration.ToDurationFormatStringWithoutSeconds();

            var calculatedHeight = (int)(calendarItem.DurationTimeSpan.TotalHours * 48) + calendarItem.DurationTimeSpan.Hours * 16;
            var height = calculatedHeight < 64 ? 64 : calculatedHeight;
            var offset = (int)(tracking.StartTime.TimeOfDay.TotalHours * 64);

            var calendarItemControl = new CalendarItemControl
            {
                CalendarItem = calendarItem,
                Visibility = Visibility.Visible,
                Height = height,
                Width = 188,
                IsReadOnly = tracking.EndTime is not null,
                DisplayDuration = calendarItem.DurationTimeSpan.ToDurationFormatStringFull(),
                ActivityName = calendarItem.ActivityName,
                StopCommand = calendarItem.StopCommand
            };

            calendarItemControl.PreviewMouseDown += OnCalendarItemPreviewMouseDown;

            var binding = new Binding(nameof(CalendarItemControl.IsSelected))
            {
                Source = calendarItem,
                Mode = BindingMode.TwoWay
            };
            calendarItemControl.SetBinding(CalendarItemControl.IsSelectedProperty, binding);

            canvas.Children.Add(calendarItemControl);
            Canvas.SetLeft(calendarItemControl, 0);
            Canvas.SetTop(calendarItemControl, offset);

            weekTotalTime += calendarItem.DurationTimeSpan;

            _calendarItemControls.Add(calendarItemControl);
        }

        _weekTotalTime.Text = weekTotalTime.ToDurationFormatStringWithoutSeconds();
    }

    private void ClearCanvases()
    {
        _mondayCanvas.Children.Clear();
        _tuesdayCanvas.Children.Clear();
        _wednesdayCanvas.Children.Clear();
        _thursdayCanvas.Children.Clear();
        _fridayCanvas.Children.Clear();
        _saturdayCanvas.Children.Clear();
        _sundayCanvas.Children.Clear();
    }

    private void ResetTimes()
    {
        _mondayTotalDuration = TimeSpan.Zero;
        _tuesdayTotalDuration = TimeSpan.Zero;
        _wednesdayTotalDuration = TimeSpan.Zero;
        _thursdayTotalDuration = TimeSpan.Zero;
        _fridayTotalDuration = TimeSpan.Zero;
        _saturdayTotalDuration = TimeSpan.Zero;
        _sundayTotalDuration = TimeSpan.Zero;
    }

    private void ClearCalendarItemControls()
    {
        foreach (var item in _calendarItemControls)
        {
            item.PreviewMouseDown -= OnCalendarItemPreviewMouseDown;
        }

        _calendarItemControls.Clear();
    }

    private Canvas GetCanvas(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => _mondayCanvas,
            DayOfWeek.Tuesday => _tuesdayCanvas,
            DayOfWeek.Wednesday => _wednesdayCanvas,
            DayOfWeek.Thursday => _thursdayCanvas,
            DayOfWeek.Friday => _fridayCanvas,
            DayOfWeek.Saturday => _saturdayCanvas,
            DayOfWeek.Sunday => _sundayCanvas,
            _ => throw new NotImplementedException(),
        };
    }

    private ref TimeSpan GetTotalDurationField(DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case DayOfWeek.Monday:
                return ref _mondayTotalDuration;
            case DayOfWeek.Tuesday:
                return ref _tuesdayTotalDuration;
            case DayOfWeek.Wednesday:
                return ref _wednesdayTotalDuration;
            case DayOfWeek.Thursday:
                return ref _thursdayTotalDuration;
            case DayOfWeek.Friday:
                return ref _fridayTotalDuration;
            case DayOfWeek.Saturday:
                return ref _saturdayTotalDuration;
            case DayOfWeek.Sunday:
                return ref _sundayTotalDuration;
            default:
                throw new NotImplementedException();
        }
    }

    private TextBlock GetTotalTimeTextBlock(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => _mondayTotalTime,
            DayOfWeek.Tuesday => _tuesdayTotalTime,
            DayOfWeek.Wednesday => _wednesdayTotalTime,
            DayOfWeek.Thursday => _thursdayTotalTime,
            DayOfWeek.Friday => _fridayTotalTime,
            DayOfWeek.Saturday => _saturdayTotalTime,
            DayOfWeek.Sunday => _sundayTotalTime,
            _ => throw new NotImplementedException(),
        };
    }

    private TextBlock GetBillableTimeTextBlock(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => _mondayBillableTime,
            DayOfWeek.Tuesday => _tuesdayBillableTime,
            DayOfWeek.Wednesday => _wednesdayBillableTime,
            DayOfWeek.Thursday => _thursdayBillableTime,
            DayOfWeek.Friday => _fridayBillableTime,
            DayOfWeek.Saturday => _saturdayBillableTime,
            DayOfWeek.Sunday => _sundayBillableTime,
            _ => throw new NotImplementedException(),
        };
    }

    private static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
    {
        DateTime jan1 = new(year, 1, 1);
        int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

        // Use first Thursday in January to get first week of the year as
        // it will never be in Week 52/53
        DateTime firstThursday = jan1.AddDays(daysOffset);
        var cal = CultureInfo.CurrentCulture.Calendar;
        int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        var weekNum = weekOfYear;
        // As we're adding days to a date in Week 1,
        // we need to subtract 1 in order to get the right date for week #1
        if (firstWeek == 1)
        {
            weekNum -= 1;
        }

        // Using the first Thursday as starting week ensures that we are starting in the right year
        // then we add number of weeks multiplied with days
        var result = firstThursday.AddDays(weekNum * 7);

        // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
        return result.AddDays(-3);
    }

    private static void OnShowOnlyWorkingDaysChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not CalendarControl control)
        {
            return;
        }

        control.SetGridColumnsWidth();
    }

    private void SetGridColumnsWidth()
    {
        if (ShowOnlyWorkingDays)
        {
            _calendarWeekDaysGrid.ColumnDefinitions[11].Width = new GridLength(0, GridUnitType.Pixel);
            _calendarWeekDaysGrid.ColumnDefinitions[12].Width = new GridLength(0, GridUnitType.Pixel);
            _calendarWeekDaysGrid.ColumnDefinitions[13].Width = new GridLength(0, GridUnitType.Pixel);
            _calendarWeekDaysGrid.ColumnDefinitions[14].Width = new GridLength(0, GridUnitType.Pixel);
            _calendarGrid.ColumnDefinitions[11].Width = new GridLength(0, GridUnitType.Pixel);
            _calendarGrid.ColumnDefinitions[12].Width = new GridLength(0, GridUnitType.Pixel);
            _calendarGrid.ColumnDefinitions[13].Width = new GridLength(0, GridUnitType.Pixel);
            _calendarGrid.ColumnDefinitions[14].Width = new GridLength(0, GridUnitType.Pixel);
        }
        else
        {
            _calendarWeekDaysGrid.ColumnDefinitions[11].Width = new GridLength(8, GridUnitType.Pixel);
            _calendarWeekDaysGrid.ColumnDefinitions[12].Width = new GridLength(188, GridUnitType.Pixel);
            _calendarWeekDaysGrid.ColumnDefinitions[13].Width = new GridLength(8, GridUnitType.Pixel);
            _calendarWeekDaysGrid.ColumnDefinitions[14].Width = new GridLength(188, GridUnitType.Pixel);
            _calendarGrid.ColumnDefinitions[11].Width = new GridLength(8, GridUnitType.Pixel);
            _calendarGrid.ColumnDefinitions[12].Width = new GridLength(188, GridUnitType.Pixel);
            _calendarGrid.ColumnDefinitions[13].Width = new GridLength(8, GridUnitType.Pixel);
            _calendarGrid.ColumnDefinitions[14].Width = new GridLength(188, GridUnitType.Pixel);
        }
    }

    private static void OnCalendarItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not CalendarControl control)
        {
            return;
        }

        if (e.OldValue is INotifyCollectionChanged oldCollection)
        {
            oldCollection.CollectionChanged -= control.OnCalendarItemsCollectionChanged;
        }

        if (e.NewValue is INotifyCollectionChanged newCollection)
        {
            newCollection.CollectionChanged += control.OnCalendarItemsCollectionChanged;
        }

        control.RenderCalendarItems();
    }

    private void OnCalendarItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        if (args.Action == NotifyCollectionChangedAction.Remove && args.OldItems is not null)
        {
            foreach (CalendarItemViewModel item in args.OldItems)
            {
                //Removed items
                item.PropertyChanged -= CalendarItemViewModelPropertyChanged;
            }
        }
        else if (args.Action == NotifyCollectionChangedAction.Add && args.NewItems is not null)
        {
            foreach (CalendarItemViewModel item in args.NewItems)
            {
                //Added items
                item.PropertyChanged += CalendarItemViewModelPropertyChanged;
            }
        }
    }

    private void CalendarItemViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        RenderCalendarItems();
    }

    private void OnCalendarItemPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not CalendarItemControl calendarItemControl)
        {
            return;
        }

        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
            calendarItemControl.IsSelected = !calendarItemControl.IsSelected;
            SelectedCalendarItems = new ObservableCollection<CalendarItemViewModel>(
                _calendarItemControls.Where(x => x.IsSelected).Select(x => x.CalendarItem));
            return;
        }

        foreach (var control in _calendarItemControls)
        {
            control.IsSelected = false;
        }

        calendarItemControl.IsSelected = true;

        SelectedCalendarItems = new ObservableCollection<CalendarItemViewModel>(
            _calendarItemControls.Where(x => x.IsSelected).Select(x => x.CalendarItem));
    }
}
