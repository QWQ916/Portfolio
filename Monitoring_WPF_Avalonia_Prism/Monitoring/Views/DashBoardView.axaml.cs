using Avalonia.Controls;
using Avalonia.Input;
using Monitoring.Models.Widgets;
using Monitoring.ViewModels;

namespace Monitoring.Views;

public partial class DashBoardView : UserControl
{
    public DashBoardView()
    {
        InitializeComponent();
    }

    private void OnItemDoubleTapped(object? sender, TappedEventArgs e)
    {
        // e.Source — элемент, по которому кликнули. Его DataContext = виджет.
        if (e.Source is Control control &&
            control.DataContext is WidgetBase widget &&
            DataContext is DashBoardVM vm)
        {
            vm.OpenWidgetCommand.Execute(widget);
        }
    }
}
