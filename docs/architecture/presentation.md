# Presentation architecture

## Status

This document records the direction validated by the menu experiment. It narrows the presentation parts of the foundation without attempting to define the final navigation system.

## Dependency map

```text
Avalonia View
    -> Presentation ViewModel
        -> Application use case / presentation extension point
            -> Core

Host composition root
    -> UI + Presentation + Application + Infrastructure adapters
```

Views bind to ViewModels and translate framework events into commands. ViewModels own UI state and reactive subscriptions, but do not resolve services, construct windows, or depend on infrastructure adapters. Application ports describe durable operations. Domain and transport models do not inherit from ViewModel types.

## ViewModel lifetime

`ViewModelBase` is the single current base class. It owns disposables created by a ViewModel through `Own`. Disposal is idempotent because the underlying `CompositeDisposable` is idempotent. This keeps subscription and command cleanup local to the object that creates them.

A separate base class for a page or a reusable visual component is deliberately not introduced yet. Such empty marker classes would be shallow modules: deleting them would not move any behavior to callers. Introduce one only after a shared invariant exists, for example:

- a page activation/deactivation protocol with cancellation;
- a stable route identity and navigation result;
- a component-level validation or resource lifetime that differs from a page.

At that point the behavior belongs behind a small interface and can justify `PageViewModelBase` or `ActivatableViewModelBase`. Domain models, application DTOs, and persisted state must remain plain types and must never derive from `ViewModelBase`.

## State ownership

| State | Owner | Consumers |
| --- | --- | --- |
| Available root menu items | `AppMenuViewModel` | Main shell ViewModel/View |
| Role availability | `CurrentUserContext` in the demo; a future session contract in the application | Access policies |
| Current demo route and heading | `AppNavigationState` | Menu contributions and shell |
| Menu contribution presentation | Concrete `MenuItemViewModel` | `MenuItemView` |

There is one writable owner for each state. Other modules observe it or invoke its interface. Messages may announce changes, but they are not used as state storage.

## Menu experiment

The menu remains an extension point made of contributions. Registration is static for an application run; access and selection are reactive. The owner filters and orders root contributions, while a container owns its child projection. Execution rechecks access so a stale visible item cannot bypass a changed policy.

The current demo intentionally keeps route content in `AppNavigationState`. It is not the final navigation seam. The next navigation experiment should replace it with a small interface such as `NavigateAsync(route, cancellationToken)` returning an explicit outcome, and should own page activation, cancellation, and local error recovery. Menu contributions should then request navigation rather than mutate page state.

## Composition and lifetime

Singleton presentation objects are disposed by the DI container in reverse dependency order. Views do not dispose ViewModels resolved from the container. A transient navigation scope may replace singleton page lifetimes later, but only when multi-window or back-stack behavior demonstrates the requirement.

## Next decisions

1. Define route values and navigation outcomes independently of Avalonia.
2. Prototype page activation, cancellation, and back-stack ownership.
3. Decide whether child menu composition needs one generalized contribution registry or should stay owner-specific.
4. Move demo identity and role state behind application-facing session contracts.
5. Add architecture tests that enforce project dependency direction.
