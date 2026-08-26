# Presentation architecture

## Project seam

`MiraiSpace.Presentation.Abstractions` is a BCL-only project for contracts consumed by independent Modules. It does not reference ReactiveUI, ReactiveUI.Validation, DynamicData, Avalonia, Eremex, DI, or `System.Reactive`.

`MiraiSpace.Presentation` implements those contracts and owns reactive composition. `MiraiSpace.UI` owns Views, Eremex adapters, and exact `IViewFor<TViewModel>` registrations.

## Base types

- `Model` is observable Presentation state without a View lifecycle.
- `ViewModelBase` implements ReactiveUI `IActivatableViewModel` and exposes protected activation and deactivation hooks.
- `Component` and `Page` are intentionally empty semantic bases. Their behavior is not guessed in advance.
- Validation is opt-in on the concrete ViewModel through ReactiveUI.Validation; it is not imposed on every ViewModel or exposed by Presentation abstractions.

Every ViewModel normally has its own View contract. A shared visual implementation may satisfy several exact `IViewFor<TConcreteViewModel>` registrations, but rendering still passes the concrete ViewModel to `ContentControl` and lets `ViewLocator` resolve its exact View.

## Initialization

`IInitializable` and `IInitializable<TParameter>` accept runtime input after DI construction. Initialization is repeatable and may run while a ViewModel is active. A new initialization cancels the previous initialization cooperatively, allowing the existing instance to rebuild for the latest input.

Construction, initialization, activation, and disposal remain distinct:

1. DI constructs stable dependencies.
2. An owner supplies runtime input through `InitializeAsync`.
3. ReactiveUI activates active-only subscriptions when the View is active.
4. A later input may reinitialize the same active instance.
5. ReactiveUI deactivation releases its activation scope and invokes `OnDeactivated`.
6. DI or the owning scope performs final disposal.

The owner initializes what it owns. Navigation initializes a Page before publishing it. A parent may initialize a known child. `Lazy<T>` represents a single lazy child; a feature-specific factory/delegate creates repeated instances. A universal ViewModel factory is deliberately rejected because it resembles a Service Locator.
