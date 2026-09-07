# Presentation architecture

## Base types

`ReactiveModel` primarily provides INPC and minimal presentation-model logic, without a View lifecycle. Domain entities, application DTOs, persisted records, and transport values do not inherit from presentation bases.

`ReactiveComponent` represents a composable part of a page. The current implementation supports ReactiveUI activation through protected `OnActivated` and `OnDeactivated` hooks. Resources added to the activation `CompositeDisposable` are released by ReactiveUI; deactivation is a synchronous notification and does not replace an explicit asynchronous close or navigation protocol.

`ReactivePage` represents the top-level page composed from components. It is currently an empty semantic specialization of `ReactiveComponent`; the navigation and initialization lifecycle is being discussed. Its current implementation must not be treated as a decision against a future initialization contract.

Validation is opt-in. A concrete ViewModel uses ReactiveUI.Validation when it needs validation; the shared base and abstractions do not force validation dependencies on read-only or non-form ViewModels.

## Initialization

The agreed interface name is `IInitializable`, without `Async` in the interface name. The method name and return type, a possible `IInitializable<TParameters>`, and the rules for cancellation, repeatability, page reuse, and disposal remain proposals. Do not impose initialization on every `ReactiveModel` or equate initialization with each View activation.

See [API agreements](api-agreements.md) for the current discussion boundary and accepted decisions. Navigation is discussed first, dialogs afterwards; implementation is paused until the next step is agreed.

## View responsibilities

Views render and adapt UI. Application actions belong to ViewModel commands and services. Prefer bindings and behaviors from wieslawsoltes/Xaml.Behaviors for control events; a necessary Eremex row-click adapter passes the row to a command, which decides whether to navigate or open a dialog. Moving application logic from code-behind into a behavior does not satisfy this boundary.

## Errors and logging

Configure ReactiveUI's common command exception handler in the application composition root. Local `ThrownExceptions` subscriptions are reserved for intentional feature-specific recovery; do not add repetitive command catches or silently consume failures. Configure standard Microsoft logging with Serilog at the host boundary and inject `ILogger<T>` where needed. Presentation does not supply `NullLogger<T>` fallbacks.

## View resolution

Every rendered concrete ViewModel has an exact `IViewFor<TViewModel>` registration. `ContentControl` supplies its content as the View DataContext; `ViewLocator` only resolves the exact View contract from DI. A reusable generic View may serve several concrete menu-item types, but each closed mapping remains explicit and plugins can replace a concrete registration deterministically.

The root `MainWindow` is resolved as a concrete scoped View. Constructor injection supplies `MainWindowViewModel`; the desktop lifetime does not cast an `IViewFor<T>` back to `Window` or resolve services from window code-behind.
