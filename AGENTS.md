# Repository instructions

- Do not run or launch application projects. If validation is necessary, build the project without launching it.
- The user requested removal of the test projects during API redesign. Do not add, regenerate, or run tests until explicitly requested.

# MiraiSpace coding preferences

- Do not use primary constructors. Prefer an explicit constructor and ordinary fields/properties.
- Put dependency-injection registration extensions in a `DependencyInjection` namespace/folder, not `Composition`.
- Keep framework-specific implementation types out of extensibility contracts. Menu contracts use `ICommand` and read-only collection interfaces, not `ReactiveCommand` or concrete observable collections.
- Compose menu items with keyed DI. Use the stable string key `Root` for top-level items and a stable container-owned string key for each container. Never use concrete `Type` objects as menu keys.
- Keep `IAppMenuItem` minimal. Optional presentation data belongs to the concrete item/view model and its view.
- Register views as exact `IViewFor<TViewModel> -> TView` services. Plug-ins register their own view mappings. The application `ViewLocator` resolves only the exact runtime view-model type; it does not walk base classes, assign `DataContext`, or create fallbacks.
- Compact menu behavior is a UI concern. Prefer declarative Avalonia properties/styles and a `Flyout` for container children; do not repeat imperative display-mode code in every view.
- Prefer ReactiveUI.SourceGenerators for reactive properties and commands. Preserve each menu item's freedom to choose sync/async execution and its own `CanExecute`.
- `AddPresentation()` is the public entry point for all Presentation registrations. It may delegate internally, but hosts should not assemble individual Presentation features.
- Prefer `var` for local variables when the assigned expression makes the type clear.
- Bind DynamicData pipelines directly to a private `readonly ReadOnlyObservableCollection<T>` field with `Bind(out _items)` and expose it through a getter. Do not make the collection property reactive/settable, and do not retain a mutable UI collection merely as a binding target. Enable `UseReplaceForUpdates` when the target controls support replace notifications.
- Use `SortAndBind` only when the pipeline actually sorts. Put its scheduler in `SortAndBindOptions`; for an unsorted `Bind` pipeline, keep the UI scheduler at the binding boundary with `ObserveOn`.
- Keep every application-menu DI key in the shared `AppMenuKeys` class and use those constants in registrations and `[FromKeyedServices]` attributes.
- Do not register Presentation view models or menu items as singletons. Use an application scope when several view models must share session state.
- Use `ReactiveUserControl<TViewModel>` (and `ReactiveWindow<TViewModel>` for windows) when ReactiveUI view activation is required. Do not introduce a custom non-generic view base solely to reproduce that lifecycle.
- Use the ReactiveUI.SourceGenerators `[IViewFor<TViewModel>]` attribute only when it removes real registration or binding code. Do not create empty per-view-model subclasses merely to specialize one reusable menu-item view; register a reusable closed generic `IViewFor<TViewModel>` instead.
- Store Avalonia attached properties and behaviors outside `Views` (for example, under `Behaviors`).
- Register the application `ViewLocator` in UI DI and resolve that registration when adding it to `Application.DataTemplates`; do not construct it manually in the host.
- Keep only extension contracts in the abstractions project. Policy base classes and other implementation helpers belong to Presentation.
- Do not add an access-policy state unless it is supported end to end. A filtering-only access policy returns `bool`; `Disabled` requires real UI and command behavior, not an unused enum value.
- Treat sample UI as production-facing UI: maintain a coherent visual system and verify both expanded and compact menu modes, including container flyouts.
- Keep `Window` code-behind free of DI scopes, service resolution, and lifetime management. Create the application scope in the application composition root and expose window content through a dedicated window view model.
- Treat an `IAppMenuItemContainer` as a menu group, not a navigation item. It owns and filters its child items, may add dynamic children, and presents them inline or in a compact-mode flyout.
- Keep `IAppMenuItem.ExecuteCommand` typed as framework-neutral `ICommand`. A source-generated `ReactiveCommand` property needs an explicit interface bridge because C# interface implementation requires an exact property type; do not remove that bridge merely because the property names match.
- Prefer source-generated partial properties over annotated backing fields, and place public properties before constructors.
- Target the desktop host for now; do not reintroduce browser host/platform projects unless explicitly requested.
- For DynamicData refiltering driven by invalidation signals, prefer the state overload `Filter(IObservable<TState>, Func<TState, TItem, bool>, ...)`; do not project every signal into a new `Func<TItem, bool>`.
- When a requested review change conflicts with the language type system or the framework lifecycle, explain the constraint and keep the correct code instead of applying the request mechanically.

# Agreed presentation direction (2026-09-06)

- Read `docs/architecture/api-agreements.md` before changing navigation, dialogs, menu contracts, presentation lifecycles, or their packages. It distinguishes user decisions from proposals. Final architecture implementation is paused for API discussion. The user explicitly authorized the binding generator installation, a small navigation command palette, and restoration of Generic Host startup; these narrow steps do not approve the remaining architecture.
- Keep Views free of application logic: no navigation decisions, dialog orchestration, business rules, service resolution, or DI lifetime ownership. Rare code-behind is limited to necessary UI adaptation. Prefer bindings and behaviors from wieslawsoltes/Xaml.Behaviors; an event adapter forwards a typed input to a ViewModel command, which owns the application action.
- Configure ReactiveUI's common command exception handler in application bootstrap. Do not repeat `try/catch` or subscribe to every command's `ThrownExceptions` merely to log or swallow cancellation. Use a local exception subscription only for deliberate feature-specific recovery; explicitly forward/report failures that still need the common policy. This policy does not cover arbitrary exceptions outside ReactiveCommand.
- Use standard Microsoft logging integration and inject `ILogger<T>`. Serilog is the agreed default provider direction. Do not register fallback `NullLogger<T>` services in Presentation or Views.
- Preserve the existing Generic Host / `CP.Extensions.Hosting.Avalonia` integration when implementing the next agreed step; discuss any hosting redesign before making it.
- `ReactiveModel` primarily provides INPC with minimal logic; `ReactiveComponent` represents a reusable part of a page; `ReactivePage` is the top-level page. Do not make lightweight models participate in a page lifecycle.
- Name the initialization interface `IInitializable`, not `IAsyncInitializable`. Do not add repeated initialization for now; explicit feature refresh methods may be added when needed. Exact signatures, parameter binding, and ownership still need agreement.
- Use MessagePipe for application messages. Do not introduce a competing custom message bus. Choose broker lifetimes explicitly when session/page ownership is agreed.
- Discuss dialogs after navigation is agreed. Dialogs do not have navigation URLs. Embedded presentation in page and component regions is now selected. Dialog ViewModels come from DI; Title is not mandatory. Exact service and region-token signatures are still proposals.
- Defer Autofac until a concrete scope/lifetime requirement needs it. Additional packages and menu API changes still require design agreement. Permission to add packages does not make a proposed package an accepted dependency.
- Avoid unnecessary fully qualified names in ordinary source code; use imports unless a real ambiguity requires qualification.
- Each menu container owns filtering and composition of its children. Similar filtering pipelines in separate containers are not, by themselves, a reason to extract a shared filtering service or relocate that responsibility. Menu/navigation integration remains under discussion; do not impose route-derived structure or selection on every item.

# Navigation discussion updates (2026-09-07)

- One navigation instance owns one current page. Each future logical floating document panel/window owns its own DI scope and navigation. Do not put Eremex controls in navigation contracts.
- The menu should show the active page while each container retains its own child composition and filtering.
- Page state/persistence belongs to a separate extended-settings discussion.
- The first example plugin should add an ordinary page. Low-cost explicit View/service replacement is acceptable in principle; its policy is still to be agreed.
- A small navigation command palette is authorized for experimentation now. AI integration, final navigation/dialog APIs, and broad dependency installation are not authorized by the library examples.

# Accepted refinements from the latest discussion (2026-09-07)

- Base page navigation on Avalonia NavigationPage/ContentPage. The user explicitly selected stack navigation with Back and no browser Forward for the first implementation.
- Do not expose CanGoBack or CanGoForward in the application navigation contract merely for button presentation. The Avalonia control may retain its own UI state.
- Region-based dialogs are required: support the whole page and a specific component instance. Identical components in different panels must have independent regions.
- Accept a dialog ViewModel created with DI; do not require callers to construct dependency-bearing dialog models with new. Do not require Title or create a separate Window for ordinary dialogs.
- Navigation and dialog APIs are still being refined; their proposed signatures are not accepted merely because they appear in a document. New demo cases must exercise required typed page parameters and actual DI-backed data loading.
- Target net10.0/C# 14 and prefer System.Threading.Lock for dedicated synchronous locks. Preserve appropriate asynchronous primitives where awaiting is required.

# First implementation authorization (2026-09-07)

- The user explicitly ended the documentation-only phase and requested the first working implementation of ViewModel-oriented native navigation, child-component initialization, DI-created embedded dialogs, and meaningful demo examples. Proceed with that implementation; earlier pause wording is superseded.
- Preserve all agreed architecture and coding preferences. Do not create more design documents instead of implementing. Existing restrictions on tests and application launch remain in force unless explicitly changed by the user.

# Review corrections (2026-09-07)

- These corrections supersede experimental implementation and older proposals: presentation navigation, dialogs and initialization belong to Presentation, not Application.
- Do not use ActivatorUtilities. Register models explicitly and resolve from the owning DI scope; no constructor argument injection via reflection.
- ReactiveComponent does not implement any initialization interface. Compose initialization separately. A parameterized model exposes only its parameterized initialization method.
- Do not make menu containers inspect/cast items to ReactiveComponent or initialize them. Preserve their filtering responsibility.
- Dialog models request closure through their own result/event contract; do not inject IDialogService merely to close themselves. No empty dialog marker interfaces and no object owner.
- Use the hosting package's plugin infrastructure and supply a real separately built sample plugin with a page, menu item and command.
- Implement scoped Eremex floating navigation panels and a shared structured command engine suitable for a future AI caller. A palette parser alone is not that engine.
- Route registration contains descriptors only: no model construction, initialization, disposal or presentation titles.
- Do not routinely dispose ReactiveCommands. Use ReactiveUI activation for UI subscriptions and explicit owned lifetime only for work that must survive deactivation.
- Put actual attached properties in AttachedProperties; put Behavior-derived UI adapters in Behaviors. Views remain declarative.
- Use one consistent DependencyInjection folder/namespace and Add... registration convention.
- Fix analyzer warnings in changed code rather than suppressing them globally.
