using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI;

namespace MiraiSpace.UI.Views.Menu;

public sealed class StandardAppMenuItemView<TItem> : StandardAppMenuItemView, IViewFor<TItem>
    where TItem : StandardAppMenuItem
{
    TItem? IViewFor<TItem>.ViewModel
    {
        get => ViewModel as TItem;
        set => ViewModel = value;
    }
}
