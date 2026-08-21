---
name: eremex-avalonia-controls
description: Integrate, configure, style, troubleshoot, or evaluate Eremex Avalonia UI Controls in a cross-platform Avalonia application. Use for Eremex package setup, control selection, AXAML integration, themes, licensing, platform compatibility, and migrations; verify proprietary APIs against the installed package and current official Eremex documentation instead of guessing.
---

# Eremex Avalonia Controls

Treat Eremex as an optional UI adapter, not as a dependency of domain or application logic.

## Workflow

1. Inspect `Directory.Packages.props`, the target `.csproj`, Avalonia version, target frameworks, and installed Eremex packages.
2. Read [official-sources.md](references/official-sources.md). Open the relevant current documentation page before writing API-specific code.
3. Check the exact installed assembly API with IDE metadata, NuGet package contents, or a small compiling probe. Do not infer an Eremex member from a similarly named Avalonia, WPF, DevExpress, or Telerik member.
4. Keep view models free of Eremex control types. Expose state, commands, and domain-friendly models; adapt them in AXAML, behaviors, converters, or a thin view service.
5. Prefer compiled bindings and explicit `x:DataType`. Keep platform-only functionality behind capabilities or interfaces.
6. Build every affected target. For a visual change, run the relevant host and capture a screenshot.

## Guardrails

- Keep all Avalonia package versions aligned through central package management.
- Pin the Eremex version centrally; review its release notes and Avalonia compatibility before upgrading.
- Confirm licensing and feed configuration without committing credentials, license keys, or private feed tokens.
- Virtualize large data sets and measure before enabling expensive templates, grouping, summaries, or live updates.
- Dispatch only UI mutations to `Dispatcher.UIThread`; perform I/O and computation off the UI thread.
- Dispose event handlers, subscriptions, and control-owned resources when a view deactivates.
- Wrap third-party controls behind a narrow view-facing seam when replacement cost or browser/mobile support is uncertain.

## Verification

Run `dotnet restore`, `dotnet build`, and focused tests. Verify desktop and browser separately when both are affected: a control supported on desktop may have different browser constraints.
