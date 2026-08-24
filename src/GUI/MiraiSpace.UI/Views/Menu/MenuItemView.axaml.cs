using Avalonia.Markup.Xaml;
using MiraiSpace.Presentation.Menu.Demo;
using ReactiveUI.Avalonia;

namespace MiraiSpace.UI.Views.Menu;

public partial class MenuItemView : ReactiveUserControl<MenuItemViewModel>
{
    public MenuItemView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
