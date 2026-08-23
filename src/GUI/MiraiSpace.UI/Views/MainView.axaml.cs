using Avalonia.Controls;
using Avalonia.Interactivity;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.ViewModels;

namespace MiraiSpace.UI.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private async void OnMenuItemClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: IAppMenuItem item }
            && DataContext is MainViewModel viewModel)
        {
            await viewModel.ExecuteAsync(item);
        }
    }
}
