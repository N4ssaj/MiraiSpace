# Presentation architecture

## Type roles

Presentation uses four deliberately different base types:

| Type | Responsibility | ReactiveUI lifecycle |
| --- | --- | --- |
| `ModelBase` | Observable presentation state that is not owned by a View | No |
| `ViewModelBase` | State and work whose lifetime follows a View | `IActivatableViewModel` and `ViewModelActivator` |
| `ComponentViewModelBase` | An independently rendered ViewModel with a stable component identity | Inherited activation |
| `PageViewModelBase` | Route-addressable shell content with a stable route and title | Inherited activation |

Domain entities, application DTOs, persisted records, and transport models inherit from none of these types. `ModelBase` is specifically a presentation model.

`ViewModelBase` does not implement a second disposal framework. It registers an activation block with ReactiveUI. Derived ViewModels override the activation method for their role and add subscriptions or bindings to the supplied `CompositeDisposable`. ReactiveUI disposes that scope when the corresponding View deactivates and creates a new scope on the next activation. `PageViewModelBase` also creates and cancels a token for page-scoped asynchronous work on every activation.

Constructors only establish immutable identity, commands, and cheap initial state. I/O, timers, realtime subscriptions, and page-scoped cancellation begin during activation.

## Ownership

- A View activates its own ViewModel through ReactiveUI.
- A shell ViewModel activates child ViewModels that it owns but that do not have an independent ReactiveUI View.
- A `ModelBase` is owned by the module that projects it. It does not pretend to have visibility lifecycle.
- DI owns singleton disposal and Generic Host owns application shutdown. View activation is shorter-lived and does not replace host shutdown.

## Menu presentation

The application menu demonstrates these roles. `AppMenuViewModel` is an activatable component. `AppMenuItemModel` is a non-activatable projection created by the menu composer. Contributions are not ViewModels and do not depend on ReactiveUI, Avalonia, or Eremex.

The Avalonia adapter renders that projection with Eremex `ListViewControl`. Eremex therefore remains replaceable UI implementation detail rather than becoming part of the extension point.
