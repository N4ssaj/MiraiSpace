using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.UI.Views.Menu;

[IViewFor(nameof(TItem))]
public sealed partial class StandardAppMenuItemView<TItem> : StandardAppMenuItemView
    where TItem : StandardAppMenuItem
;
