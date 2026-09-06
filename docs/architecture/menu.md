# Application menu

## Contracts

Extension contracts live in `MiraiSpace.Extensibility.Abstractions`:

- `IAppMenuItem` exposes only an `ICommand`;
- `IAppMenuItemContainer` additionally exposes a read-only list of child items;
- `IAppMenuAccessPolicy` evaluates access and publishes an invalidation signal;
- `AppMenuKeys` contains the stable DI keys for the root menu and each container.

Item-specific presentation state such as title, icon, caption, badge, expansion, or dynamic children belongs to the concrete item ViewModel. ReactiveUI commands implement the framework-neutral `ICommand` contract through an explicit interface bridge.

## Ownership and composition

Root contributions are registered as keyed `IAppMenuItem` services under `AppMenuKeys.Root`. A container receives contributions registered under its own stable key; no parent ids or concrete implementation types are used as keys.

`AppMenuViewModel` and each container own their DynamicData pipelines. A pipeline filters its source when access policies are invalidated, binds directly to a private `ReadOnlyObservableCollection<IAppMenuItem>`, and exposes that collection through a getter. A container may also add or remove runtime children in its own source list.

Policy base classes and filtering implementation remain in Presentation. The extension project contains only contracts required by contributors.

## Rendering

Avalonia binds menu collections to `ItemsControl`. The application `ViewLocator` resolves the exact `IViewFor<TConcreteViewModel>` registration from DI; it does not walk base types, create fallbacks, or assign a DataContext.

Reusable standard item visuals use a closed generic `StandardAppMenuItemView<TItem>`. ReactiveUI.SourceGenerators produces the matching `IViewFor<TItem>` implementation, while DI still contains an explicit exact registration for each concrete item type.

Compact state is an inherited Avalonia attached property under `Behaviors`. Styles adapt shared item visuals declaratively, and containers expose their children through a `Flyout` in compact mode. No compact-mode state or imperative layout switching is added to menu ViewModels.
