using Avalonia.Controls;
using Avalonia.Interactivity;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.ViewModels;

namespace MiraiSpace.UI.Views;

public partial class MainView : UserControl
{
    public MainView(ViewLocator viewLocator, AppMenuItemChildrenSelector childrenSelector)
    {
        Resources.Add("MenuItemViewLocator", viewLocator);
        Resources.Add("AppMenuItemChildrenSelector", childrenSelector);
        InitializeComponent();
    }

    private void OnMenuItemClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: IAppMenuItem item }
            && DataContext is MainViewModel viewModel)
        {
            viewModel.ExecuteMenuItemCommand.Execute(item).Subscribe();
        }
    }
}
