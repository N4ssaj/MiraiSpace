# Presentation API agreements

Recorded from the user's discussion on 2026-09-06. Accepted direction and open proposals are separate. Existing experimental code does not establish an accepted API.

## Current boundary

Final navigation, initialization, and dialog API implementation remains paused for discussion. Explicitly authorized narrow steps are the binding generator installation, a small navigation command palette, and restoration of the original Generic Host startup. These steps do not approve the remaining experimental architecture or all example packages. Repository restrictions on tests and application launch still apply.

## Accepted direction

- Command errors: configure ReactiveUI's common command exception handler at bootstrap. Do not repeat catches or subscribe to every command's ThrownExceptions merely for logging. Local handling is reserved for deliberate feature-specific recovery; explicitly forward/report failures that still need the common policy. This handler does not cover arbitrary exceptions elsewhere in the process.
- Views: no application logic, navigation decisions, dialog orchestration, service location, or scope ownership. Rare necessary UI adaptation is allowed. Prefer bindings and behaviors from wieslawsoltes/Xaml.Behaviors. An Eremex row-click adapter forwards the row to a ViewModel command, which owns the application action.
- Logging: standard Microsoft logging integration with ILogger<T>; Serilog is the default provider direction. Do not register fallback NullLogger<T> services in Presentation or Views.
- Hosting: preserve the intended Generic Host integration using CP.Extensions.Hosting.Avalonia. Discuss any redesign first. The user explicitly requested restoration; the original Generic Host / ConfigureAvalonia / UseAvaloniaLifetime structure has now been restored.
- ReactiveModel: lightweight presentation model, primarily INPC with minimal logic.
- ReactiveComponent: reusable component from which a page is composed.
- ReactivePage: top-level page. Navigation lifetime and component activation lifetime must be considered separately.
- Initialization naming: IInitializable, without Async in the interface name. This does not settle the method signature or require every base type to implement it.
- Messages: use MessagePipe instead of a competing custom bus. Publish facts; navigation and dialog calls retain explicit service contracts. Broker ownership remains to be chosen with session/page lifetimes.
- Dialogs: no navigation URLs. Embedded dialogs must support page-wide and component-specific regions. Pass ViewModels created with DI; Title is not mandatory. Exact result/session and region-token APIs remain proposals; ordinary dialogs must not default to separate Window instances.
- Code style: avoid unnecessary fully qualified names. Existing source-generator, exact View mapping, keyed menu DI, and property-order preferences remain in AGENTS.md.

## Navigation decisions still open

- URL syntax, typed parameter binding, and navigation result/exception semantics.
- Use Avalonia NavigationPage/ContentPage with one current page per navigation instance. The user selected ordinary stack navigation with Back and no Forward for the first implementation.
- Back returns through the native NavigationPage stack; do not repeatedly initialize a retained page. Cross-run persistence remains a separate topic.
- Each logical panel/floating window owns its own DI scope and navigation. Finer page/dialog resource ownership within that scope still needs design.
- Repeated initialization is deferred. Initialization timing, feature-specific refresh, cancellation, and overlapping navigation still need exact semantics.
- Unsaved-change guards and which expected outcomes should be returned rather than thrown.
- The menu should indicate the active page. Exact matching and integration with the active panel still need a contract, preserving minimal IAppMenuItem and container-owned filtering.

Possible initialization shape for discussion: IInitializable and IInitializable<TParameters> with ValueTask-returning methods and cancellation. A typed interface makes its accepted parameter type explicit; a generic method accepting every T does not provide that constraint. Neither signature nor placement in the base hierarchy is accepted yet.

## Package discussion

| Package/family | Status | Reason and boundary |
| --- | --- | --- |
| MessagePipe | Selected direction | Typed messages; choose broker lifetime explicitly. |
| Serilog.Extensions.Hosting and required sinks/configuration | Selected direction | Host logging behind ILogger<T>. |
| wieslawsoltes/Xaml.Behaviors | Preferred UI adaptation library | Choose required packages and check compatibility with installed Avalonia before adding them. |
| Autofac + Autofac.Extensions.DependencyInjection | Deferred until needed | The user selected postponement until an actual scope/lifetime requirement needs it. |
| ReactiveUI.Validation | Optional later | Feature-level form validation without burdening shared base types. |
| DialogHost / MVVMDialogs | References for later | Evaluate embedded presentation and ownership after navigation; no dependency selected yet. |

Autofac integrates with Generic Host and existing IServiceCollection registrations. It does not itself define plugin discovery, isolation, unloading, or application lifetime policy. If selected, decide how modules express lifetime requirements without unnecessary container-specific public contracts.

Inspect the existing CP.Extensions.Hosting family before inventing overlapping host/plugin lifecycle code. Its presence does not automatically select another package from that family.

## Verified references

- [ReactiveUI](https://github.com/reactiveui/ReactiveUI): verify exception bootstrap APIs against the installed version.
- [MessagePipe](https://github.com/Cysharp/MessagePipe).
- [Xaml.Behaviors](https://github.com/wieslawsoltes/Xaml.Behaviors).
- [Serilog hosting integration](https://github.com/serilog/serilog-extensions-hosting).
- [Autofac lifetime scopes](https://autofac.readthedocs.io/en/latest/lifetime/working-with-scopes.html) and [Generic Host integration](https://autofac.readthedocs.io/en/latest/integration/netcore.html).
- [Extensions.Hosting](https://github.com/reactivemarbles/Extensions.Hosting).
- [ReactiveUI.Validation](https://github.com/reactiveui/ReactiveUI.Validation).

Check current primary documentation and centrally managed package versions before proposing concrete library calls. Research references do not imply package installation.
## Later discussion updates

- Each menu container retains responsibility for filtering its own children. Repeated pipeline code does not justify extracting that policy or ownership.
- Menu activation, navigation state, and local container selection are related but distinct. Showing the active page is accepted; the matching API remains under discussion.
- The explicitly requested binding generator dependency is installed: ReactiveUI.Binding and ReactiveUI.Binding.SourceGenerators 3.4.0 in Presentation and UI. Existing WhenAnyValue call sites have not migrated to the new engine. Desktop build succeeded with 0 errors and 65 NuGet/analyzer warnings; applications and tests were not launched.
- AI-accessible application actions, plugin design, and a possible custom theme are proposals, not implementation approval.
- See [round-two proposals](api-proposals-round-2.md) and [library review](library-review.md). Their example APIs and recommendations are not accepted contracts.

## Latest user decisions and limited implementation (2026-09-07)

- One navigation instance has one current page; each logical floating document panel/window owns its own scope. Later discussion selected native NavigationPage stack behavior with Back and no Forward. Eremex DockManager remains a candidate UI adapter, not a public navigation contract.
- Page state and persistence belong to a separate extended-settings discussion. Do not repeatedly request a persistence decision in this round.
- First demonstration plugin: add an ordinary new page. Permit explicit View/service replacement if inexpensive; the override contract is still open.
- Build a small command palette to exercise navigation now; AI integration is deferred. Library links are references, not authorization to install everything.
- Implemented the limited palette (Ctrl+K, go /path, back, forward, help) and restored Generic Host startup. One bootstrap subscription connects the common ReactiveUI WithExceptionHandler to command error state and ILogger. Views contain bindings; focus adaptation is in a behavior.
- Desktop build completed with 0 errors and 43 warnings. No application launch or tests were performed. Existing experimental navigation lifecycle still needs shutdown validation; compilation does not establish runtime correctness.
- Read [navigation and dialog proposal](navigation-dialogs-proposal.md) for the next discussion. The latest accepted directions are recorded below; precise service signatures remain proposals.

## Confirmed refinements and audit (2026-09-07)

- Native navigation: use Avalonia NavigationPage/ContentPage; stack with Back, no browser Forward. Remove CanGoBack/CanGoForward from the proposed application contract; native UI state may still drive presentation.
- Dialogs: accept DI-created ViewModels and support page/component regions. Region identity belongs to a particular owner/component instance. Title is not required. The proposed replacement for Completion and the exact close API are not yet accepted.
- Initialization: do not add repeat initialization now; use feature-specific update methods when needed.
- Panel lifetime: one logical panel/floating window, one scope, one navigation. Defer Autofac until necessary.
- Demo: add a meaningful required-parameter scenario, such as loading a document by typed id, plus dialogs in the page and editor regions. Existing parameterized routes only change generic presentation text and are insufficient.
- Audit: the project already targets .NET 10/C# 14 and Avalonia 12.1.1. Corrected three synchronous object lock fields to System.Threading.Lock. Desktop build log confirms 0 errors and 19 warnings; telemetry was disabled for that invocation after its build-task log failed with access denied. Application/tests were not run.
- InMemoryEventBus, old dialog requests and window-based dialogs, local exception catches, and the custom navigation service remain in the experimental code. Native navigation and the new demo are not implemented in this discussion step.
