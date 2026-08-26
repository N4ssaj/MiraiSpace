using System.Collections;
using Eremex.AvaloniaUI.Controls.TreeList;
using MiraiSpace.Presentation.Abstractions.Menu;

namespace MiraiSpace.UI.Views.Menu;

public sealed class AppMenuChildrenSelector : ITreeListChildrenSelector
{
    public bool HasChildren(object? item) =>
        item is IAppMenuItemContainer { HasChildren: true };

    public IEnumerable? SelectChildren(object? item) =>
        (item as IAppMenuItemContainer)?.Items;
}
