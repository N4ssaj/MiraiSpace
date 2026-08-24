using System.Collections;
using Eremex.AvaloniaUI.Controls.TreeList;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.UI.Views;

public sealed class AppMenuItemChildrenSelector : ITreeListChildrenSelector
{
    public bool HasChildren(object? item) =>
        item is IAppMenuItemContainer { Items.Count: > 0 };

    public IEnumerable? SelectChildren(object? item) =>
        (item as IAppMenuItemContainer)?.Items;
}
