namespace TimeTracker.WPF.Services;

public class ApplicationSettings
{
    public ApplicationSettings(
        double billablePercentage,
        bool showOnlyWorkingDays)
    {
        BillablePercentage = billablePercentage;
        ShowOnlyWorkingDays = showOnlyWorkingDays;
    }

    public double BillablePercentage { get; }

    public bool ShowOnlyWorkingDays { get; }

    public Dictionary<string, string> AllSettings => new Dictionary<string, string>
    {
        { nameof(BillablePercentage), BillablePercentage.ToString() },
        { nameof(ShowOnlyWorkingDays), ShowOnlyWorkingDays.ToString() }
    };
}
