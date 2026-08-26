# Foundation implementation roadmap

This is an evidence-driven execution order for issue #10, not a frozen feature roadmap. A step may refine the foundation before later experiments build on it.

## 1. Composition and lifecycle — started

- [x] Shared `IAppModule` entry point for built-in modules and future trusted plugins.
- [x] Explicit built-in module composition through `IServiceCollection`.
- [x] Generic Host starts inside Avalonia initialization and stops on desktop exit.
- [x] Architecture tests enforce the first dependency-direction rules.
- [ ] Extract host composition so Desktop and Browser can select modules and platform adapters independently.
- [ ] Add lifecycle tests with a recording hosted service.

## 2. Plugin startup lifecycle — next

- [ ] Define and validate the manifest contract.
- [ ] Discover candidates deterministically by stable plugin id.
- [ ] Load the shared `IAppModule` entry point and isolate recoverable discovery failures.
- [ ] Add temporary-directory integration tests for valid, malformed, incompatible, and missing-entry-point plugins.

## 3. Reactive ownership and communication

- [x] Validate reactive root access and owner-controlled container collections.
- [ ] Introduce the application session owner and move demo roles behind it.
- [ ] Add a small fact-message seam only when a second independent consumer demonstrates it.
- [ ] Add shared connection state and owner-driven reconnect refresh.

## 4. Navigation and page activation

- [x] Establish ReactiveUI activation/deactivation plus repeatable initialization contracts.
- [ ] Define route values, navigation outcomes, not-found state, and activation-error state.
- [ ] Create navigation-owned page scopes and initialize Pages before publication.
- [ ] Demonstrate a contributed page and a localized plugin failure.

## 5. View resolution and overrides

- [x] Resolve exact `IViewFor<TViewModel>` contracts from DI.
- [ ] Diagnose multiple overrides while preserving deterministic last-registration behavior.
- [ ] Decide standard-view fallback from implementation evidence.

Feature experiments from `docs/BACKLOG.md` begin only after the foundation seam they require is observable and tested.
