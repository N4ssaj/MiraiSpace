using Avalonia.Markup.Xaml;
using MiraiSpace.Presentation.Menu.Demo;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace MiraiSpace.UI.Views.Menu;

public partial class MenuItemView : ReactiveUserControl<MenuItemViewModel>
{
    public MenuItemView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

public sealed class MenuItemView<TViewModel> : MenuItemView, IViewFor<TViewModel>
    where TViewModel : MenuItemViewModel
{
    TViewModel? IViewFor<TViewModel>.ViewModel
    {
        get => DataContext as TViewModel;
        set => DataContext = value;
    }
}
