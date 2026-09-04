using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI.Avalonia;

namespace MiraiSpace.UI.Views.Menu;

public sealed class StandardAppMenuItemView<TItem> : ReactiveUserControl<TItem>
    where TItem : StandardAppMenuItem
{
    public StandardAppMenuItemView()
    {
        Content = new StandardAppMenuItemContent();
    }
}
