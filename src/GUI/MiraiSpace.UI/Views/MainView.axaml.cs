using Eremex.AvaloniaUI.Controls.ListView;
using MiraiSpace.Presentation.Menu;
using MiraiSpace.Presentation.ViewModels;
using ReactiveUI.Avalonia;

namespace MiraiSpace.UI.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
    }

    private void OnMenuItemClick(object? sender, ListViewItemClickEventArgs e)
    {
        if (e.Item.DataContext is AppMenuItemModel item
            && DataContext is MainViewModel viewModel)
        {
            viewModel.ExecuteMenuItemCommand.Execute(item).Subscribe();
        }
    }
}
