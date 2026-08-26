# Architecture foundation

## Purpose

MiraiSpace is a client-focused reference application for testing architectural ideas for complex modular applications. Document-oriented enterprise workflows may serve as demonstrations, but MiraiSpace is not a product tied to a single domain or audience.

The application is desktop-first. Browser, mobile, and other Avalonia targets are explored where practical, but they must not prevent a strong desktop implementation. Platform-specific capabilities and plugin-loading mechanisms may differ by host.

## Application lifecycle

Avalonia owns the application and UI lifecycle. A Generic Host runs inside that lifecycle and provides dependency injection, configuration, logging, hosted services, and graceful shutdown. Each host integrates these lifecycles as its platform requires; modules do not depend on the concrete Avalonia lifetime.

## Modules and plugins

- Functionality is delivered by modules as registrations and contributions.
- Built-in modules are composed explicitly through `IServiceCollection`.
- A trusted external plugin uses the same module entry point as a built-in module.
- External plugins are discovered and loaded at startup; runtime installation, hot reload, and physical assembly unloading are not foundation requirements.
- Plugin metadata lives in a manifest so compatibility and entry-point information can be checked before module registration.
- Plugins may use Avalonia when they provide custom UI. A future UI-framework replacement may be a breaking release.
- Plugin failures are isolated where practical. Discovery, navigation, command, subscription, and page boundaries should report a local error rather than terminate the application when recovery is safe.
- Trusted plugin code shares the application's privileges and can still corrupt process state; the plugin system is not a security sandbox.

`IServiceCollection` remains the composition mechanism. Extension methods may hide repetitive registration details, but the architecture does not introduce a restricted DI facade without a demonstrated need.

## Extension model

Modules extend presentation models rather than manipulating Avalonia visual trees. An owner publishes a contract, consumes all registered contributions, and exposes their composed state to its View through ordinary binding such as `ItemsSource`.

Views may be registered and replaced through `IViewFor<TViewModel>`. Built-in registrations provide defaults; plugin registrations are applied afterwards and may replace them using normal DI resolution. Plugin loading is deterministic by stable identifier, with optional ordering metadata available only when necessary.

A module may extend another module only through contracts deliberately published by the owner. Modules do not depend on another module's implementation types or reach into named UI controls.

## Reactive state and communication

Composition is static for one application run, while availability and state are dynamic. Contributions are registered even when the current user cannot see or execute them; roles, permissions, feature state, badges, and collections update reactively.

- Each kind of state has one owner.
- MessagePipe-style in-memory messages announce facts that occurred; they do not replace durable state.
- A late subscriber reads current state from its owning contract instead of waiting for another event.
- ReactiveUI represents ViewModel properties and commands.
- DynamicData manages changing collections, projections, filtering, and sorting.
- Infrastructure does not depend on the Avalonia dispatcher. Presentation code crosses to the UI scheduler when publishing bound state.
- Contribution constructors remain lightweight; I/O and long-running work belong in activation or lifecycle services.

Realtime reconnect handling belongs to the owner of each state. A restored connection may initially trigger a full refresh; incremental synchronization is added only for demonstrated scenarios.

## Dependency direction

- **Core** contains domain concepts and does not depend on Application, Presentation, Infrastructure, Avalonia, ReactiveUI, DI, or the plugin runtime.
- **Application** depends on Core and declares the ports required by use cases. It does not depend on Avalonia or concrete infrastructure.
- **Presentation** depends on application and presentation contracts. It may use ReactiveUI and DynamicData but not concrete infrastructure or Avalonia controls.
- **Avalonia UI** depends on Presentation and contains Views, controls, themes, and Avalonia adapters.
- **Infrastructure** implements Application ports and does not depend on UI.
- **Platform adapters** implement capability contracts for a particular host or operating system.
- **Hosts** are composition roots and may reference the implementations needed to assemble an application.
- **Modules** follow the same dependency direction and communicate through published contracts.
- **Plugin runtime** depends on plugin abstractions and composition contracts, not on feature implementations.

These rules should be enforced by architecture tests where project boundaries can express them. Physical project splitting remains pragmatic: a small module may use one assembly, while contracts or UI move into separate assemblies when dependency or deployment boundaries justify it.

## Deliberately open decisions

The foundation does not prescribe the final design of navigation, menus, tables, profiles, settings, docking, floating panels, multi-window support, offline synchronization, mobile lifecycle, or other future experiments. It also does not define a final public plugin SDK or require every module to expose contracts.

Those capabilities are explored independently and may refine this foundation when implementation provides evidence.
