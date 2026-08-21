# MiraiSpace agent instructions

## Repository skills

Project-local skills live in `.agents/skills/`. Use the matching skill whenever its description fits the task.

- Start ambiguous feature or architecture work with `grill-me`, `grill-with-docs`, or `domain-modeling`.
- Apply `codebase-design`, `dotnet-client-patterns`, and `improve-codebase-architecture` when changing boundaries or selecting patterns.
- Apply `avalonia`, `reactiveui`, and `eremex-avalonia-controls` for their respective UI technologies.
- Apply `dotnet-cheatsheet`, `msbuild-csproj`, `editorconfig`, `extensions-dependency-injection`, and `generic-host` for .NET infrastructure work.
- Use `tdd`, `diagnosing-bugs`, and `code-review` for implementation feedback loops.

When framework behavior or APIs may have changed, consult the current official sources linked by the relevant skill. Do not guess proprietary Eremex APIs.

## Architecture

- Keep domain and application logic independent of Avalonia, ReactiveUI, Eremex, and host-specific APIs.
- Keep desktop and browser composition roots explicit and place platform capabilities behind small interfaces.
- Preserve nullable reference types and central package management.
- Keep Avalonia package versions synchronized in `Directory.Packages.props`.
- Prefer compiled Avalonia bindings with explicit `x:DataType`.

## Verification

- Run `dotnet build MiraiSpace.sln` after code or project-file changes.
- Run focused tests when present, and add regression tests for behavior changes.
- Launch every affected host for perceptible UI changes and capture a screenshot.
