// (c) Johannes Wolfgruber, 2022

using System.Windows.Controls;
using TimeTracker.WPF.ViewModels;

namespace TimeTracker.WPF.Views;

public partial class CalendarOverviewView : UserControl
{
    public CalendarOverviewView()
    {
        InitializeComponent();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (
            sender is not DataGrid dataGrid
            || DataContext is not CalendarOverviewViewModel viewModel
        )
        {
            return;
        }

        //viewModel.SelectedTrackings = dataGrid.SelectedItems.Count == 0
        //    ? null
        //    : new ReadOnlyObservableCollection<object>(new ObservableCollection<object>(dataGrid.SelectedItems.Cast<object>()));
    }
}
