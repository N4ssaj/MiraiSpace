# Application menu

## Interface

Modules contribute `IAppMenuContribution` instances through the ordinary DI collection. Each contribution exposes:

- an `AppMenuItemDescriptor` with stable id, optional parent id, deterministic order, and standard presentation;
- a `Changed` fact stream when its descriptor presentation changes;
- one cancellable execution entry point.

The interface intentionally does not expose child collections, access decisions, selection, ReactiveUI types, or UI-framework types. Parent ids allow all modules to use one flat registration seam without owner-specific keyed-service knowledge.

## Owners

| Concern | Owner |
| --- | --- |
| Registration | Module composition |
| Descriptor and action | Contribution |
| Hierarchy validation and projection | `AppMenuViewModel` |
| Access policy aggregation | `AppMenuAccessEvaluator` |
| Last-moment authorization and execution | `AppMenuContributionExecutor` |
| Current destination | `IAppMenuSelectionSource` adapter over the navigation owner |
| Selection projection | `AppMenuViewModel` |
| Rendering and interaction adaptation | Avalonia UI |

## Invariants and failure modes

- Contribution ids are unique for one application run.
- A parent must exist, a contribution cannot parent itself, and cycles are rejected.
- Siblings sort by `Order` and then stable id.
- If a parent is inaccessible, its descendants are not projected.
- Access is checked during projection and again immediately before execution.
- Contribution constructors remain cheap; unavailable contributions are still registered.
- A presentation change announces a fact through `Changed`; consumers then read the current descriptor.

The current demo action still updates the demo navigation owner directly. The menu only observes it through `IAppMenuSelectionSource`; the navigation experiment will replace the action-side call with the published navigation interface and explicit navigation outcomes.
