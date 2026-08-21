# Pattern selection guide

| Problem | Prefer | Avoid |
| --- | --- | --- |
| Presenting state and actions | MVVM with compiled bindings | Business logic in code-behind |
| Constructing object graphs | One composition root per host, constructor injection | Service locator calls throughout the app |
| Platform differences | Capability interfaces and host-specific adapters | Scattered `#if` branches |
| Derived or time-based UI state | Observable composition and `ReactiveCommand` | Nested event handlers and mutable flag webs |
| Simple editable form | Plain properties plus commands | Reactive machinery without a real stream |
| Navigation | Coordinator/router behind a small interface | View models constructing windows or controls |
| Remote or persisted data | Repository/gateway at the infrastructure boundary | Persistence models leaking into views |
| Multiple implementations selected by state | Strategy | Large type switches |
| Object creation with invariant-heavy setup | Factory | Partially initialized public objects |
| Cross-cutting UI reactions | Decorator, behavior, or observable pipeline | A global base class containing unrelated features |
| Complex stateful workflow | Explicit state machine | Many interacting boolean flags |

## Review questions

- Does the abstraction have at least one meaningful policy or replaceable boundary?
- Can the view model run in a unit test without Avalonia initialization?
- Are cancellation and disposal owned by the same lifetime that starts the work?
- Can a host omit an unsupported capability without lying through a stub?
- Is reactive state derived from a single source of truth rather than mirrored mutable properties?
- Does a third-party control remain replaceable without rewriting domain or application logic?
