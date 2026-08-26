# Presentation architecture

## Base types

`ViewModelBase` is the only behavioral base. It implements ReactiveUI activation and exposes protected `OnActivated` and `OnDeactivated` hooks. Resources added to the activation `CompositeDisposable` are released by ReactiveUI; deactivation is a synchronous notification and does not replace an explicit asynchronous close/navigation protocol.

`Component` and `Page` are intentionally empty semantic bases. They do not add identifiers, routes, titles, duplicate activation hooks, or implicit cancellation. Behavior will be added only when a component- or page-specific invariant is demonstrated.

`ModelBase` is observable presentation state without a View lifecycle. Domain entities, application DTOs, persisted records, and transport values do not inherit from presentation bases.

Validation is opt-in. A concrete ViewModel uses ReactiveUI.Validation when it needs validation; the shared base and abstractions do not force validation dependencies on read-only or non-form ViewModels.

## Initialization

The BCL-only abstractions expose `IInitializable` and `IInitializable<TParameter>`. Initialization supplies replaceable runtime input; it is not construction and may run again while a ViewModel is active.

`ViewModelBase.InitializeLatestAsync` implements the common latest-request-wins mechanism: a new initialization cancels the previous request, while the concrete ViewModel decides how to prepare and atomically publish its new state. Owners call initialization explicitly. Navigation initializes a page before publishing it; a parent initializes children it owns. `Lazy<T>` supports one known lazy dependency, while repeated instances use a narrow feature-specific factory rather than a universal ViewModel factory.

## View resolution

Almost every concrete ViewModel has an exact `IViewFor<TViewModel>` registration. Views receive concrete ViewModels through `ContentControl`; `ViewLocator` resolves the exact View contract from DI. A standard visual may be registered for several concrete types through a registration helper, but resolution remains exact and plugins can replace a concrete registration deterministically.
