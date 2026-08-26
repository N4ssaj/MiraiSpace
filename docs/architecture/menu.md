# Application menu

## Contracts

The BCL-only menu interface is deliberately polymorphic:

- `IAppMenu` publishes root items.
- `IAppMenuItem` exposes an `ICommand`.
- `IAppMenuItemContainer` is an ordinary item that additionally owns child items.
- `IOrderedAppMenuItem` is an optional ordering hint rather than a requirement for every item.

There are no descriptors, parent ids, reactive streams, DynamicData types, access policies, or UI-framework types in the abstractions.

## Ownership

`AppMenu` owns only root composition. A container owns its children and decides when to create, show, hide, initialize, and dispose them. The root never flattens or reparents another owner's children.

Owners use DynamicData internally and bind to `ReadOnlyObservableCollection<T>`, while publishing it as `IReadOnlyList<T>`. An injected `IComparer<IAppMenuItem>` is the ordering extension point. The default comparer understands optional `IOrderedAppMenuItem`; an owner or Plugin may replace the comparer without enlarging the item interface.

A standard menu item implements its action with `ReactiveCommand`, exposed as BCL `ICommand`. `CanExecute` controls interaction. Authorization still belongs to the invoked Application use case; menu visibility is not a security boundary.

## Rendering

Eremex `TreeListControl` receives roots plus `AppMenuChildrenSelector`. The selector reports and returns children only for `IAppMenuItemContainer`. Eremex cell templates receive cell data, so the application ViewModel is passed as `Row`:

```xml
<ContentControl Content="{Binding Row}" />
```

`ContentControl` delegates to `ViewLocator`, which resolves exact `IViewFor<TConcreteViewModel>` registrations. A standard View can be registered for several concrete types, while any item or Plugin can replace one exact View.

The shell uses Avalonia `SplitView`: the same tree supports a full pane and a compact icon pane without changing menu contracts or ViewModels.
