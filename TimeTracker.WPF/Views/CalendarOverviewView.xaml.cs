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
        if (sender is not ListView listView || DataContext is not CalendarOverviewViewModel viewModel)
        {
            return;
        }

        viewModel.SelectedTrackings = listView.SelectedItems.Cast<TrackingViewModel>();
    }
}