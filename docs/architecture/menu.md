# Application menu

## Contracts

Menu contracts live in the BCL-only `MiraiSpace.Presentation.Abstractions` project:

- `IAppMenu` publishes root items;
- `IAppMenuItem` publishes an `ICommand`;
- `IAppMenuItemContainer` is an ordinary item that additionally owns child items;
- `IOrderedAppMenuItem` is an optional ordering hint;
- `IAppMenuItemComparer` is the owner-replaceable ordering strategy.

The contracts contain no descriptors, parent ids, ReactiveUI, DynamicData, System.Reactive, Avalonia, Eremex, DI, or access-policy types.

## Ownership and composition

The root menu and each container own a DynamicData pipeline internally. They bind it to `ReadOnlyObservableCollection<IAppMenuItem>` and expose it as `IReadOnlyList<IAppMenuItem>`. Access filtering and ordering are implementation concerns. The default comparer uses optional order hints and returns equality for unordered items so registration order remains the fallback.

A container decides when to create, initialize, show, hide, and release its children. Other modules extend a container through an owner-published typed item contract rather than global parent ids or implementation access.

## Rendering

Avalonia renders roots with Eremex `TreeViewControl`. `AppMenuChildrenSelector` reads `IAppMenuItemContainer.Items`; the menu owner never flattens the tree. Eremex cell templates bind `CellData.Row` into a `ContentControl`, after which `ViewLocator` resolves `IViewFor<TConcreteViewModel>`.

The shell uses `SplitView` for full and compact modes. Menu display mode is inherited UI state, not ViewModel state. Concrete item Views adapt their layout: full mode shows title/caption/badge, compact mode shows the icon and badge. Closing the pane collapses tree nodes so compact mode remains a root navigation rail.
