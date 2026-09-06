# Presentation architecture

## Base types

`ReactiveModel` is observable presentation state without a View lifecycle. Domain entities, application DTOs, persisted records, and transport values do not inherit from presentation bases.

`ReactiveComponent` implements ReactiveUI activation and exposes protected `OnActivated` and `OnDeactivated` hooks. Resources added to the activation `CompositeDisposable` are released by ReactiveUI; deactivation is a synchronous notification and does not replace an explicit asynchronous close or navigation protocol.

`ReactivePage` is an intentionally empty semantic specialization of `ReactiveComponent`. It does not add identifiers, routes, titles, initialization, or implicit cancellation. Behavior is added only when a page-specific invariant is demonstrated.

Validation is opt-in. A concrete ViewModel uses ReactiveUI.Validation when it needs validation; the shared base and abstractions do not force validation dependencies on read-only or non-form ViewModels.

## Initialization

Initialization is not imposed by a shared base contract. A feature that needs parameters, repeatable loading, cancellation, or async close behavior defines those semantics at its own boundary instead of coupling unrelated ViewModels to one lifecycle protocol.

## View resolution

Every rendered concrete ViewModel has an exact `IViewFor<TViewModel>` registration. `ContentControl` supplies its content as the View DataContext; `ViewLocator` only resolves the exact View contract from DI. A reusable generic View may serve several concrete menu-item types, but each closed mapping remains explicit and plugins can replace a concrete registration deterministically.

The root `MainWindow` is resolved as a concrete scoped View. Constructor injection supplies `MainWindowViewModel`; the desktop lifetime does not cast an `IViewFor<T>` back to `Window` or resolve services from window code-behind.
