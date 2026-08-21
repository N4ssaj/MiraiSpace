---
name: dotnet-client-patterns
description: Design or refactor maintainable cross-platform .NET client applications using MVVM, dependency injection, reactive flows, navigation, persistence, error handling, and testable boundaries. Use for Avalonia client architecture, choosing design patterns, reviewing coupling, or deciding between ReactiveUI and simpler MVVM approaches.
---

# .NET Client Patterns

Choose the smallest pattern that removes a demonstrated source of coupling. Do not create layers or abstractions only to match a diagram.

## Workflow

1. Identify supported hosts, user journeys, domain boundaries, offline needs, concurrency, and lifetime requirements.
2. Map dependencies inward: views depend on view models; view models depend on application-facing interfaces; infrastructure implements those interfaces. Keep domain code independent of Avalonia, Eremex, storage, and HTTP.
3. Select patterns from [pattern-guide.md](references/pattern-guide.md) based on the problem and listed warning signs.
4. Define composition roots per host. Share registrations where possible, but let Desktop and Browser provide different platform services.
5. Model cancellation, retries, progress, errors, and disposal explicitly. Never use `async void` except framework event handlers.
6. Test behavior through public seams. Prefer fakes for owned interfaces and focused integration tests for serialization, storage, HTTP, and controls.
7. Validate with build, tests, analyzers, and a real launch of every affected host.

## Project defaults

- Preserve nullable reference types and central package management.
- Prefer constructor injection; avoid service locator access inside views and view models.
- Prefer immutable domain values and explicit state transitions.
- Use ReactiveUI when observable composition, cancellation, derived state, or activation materially simplifies the feature. Use ordinary properties and commands for simple forms.
- Keep navigation as an application-level operation returning an outcome, not direct window construction inside a view model.
- Put platform capabilities such as clipboard, file picker, notifications, and secure storage behind small interfaces.
- Keep UI state separate from cached or persisted domain state.

Consult [official-sources.md](references/official-sources.md) before adopting or upgrading framework-specific APIs.
