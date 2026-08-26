# MiraiSpace

MiraiSpace is an architectural test bed for a modular, desktop-first application foundation. It validates reusable approaches rather than serving a particular end-user domain.

## Language

**Reference application**:
The working application in which architectural ideas are exercised and evaluated before they are reused elsewhere.
_Avoid_: Product, framework, SDK

**Module**:
A set of services and contributions that extends the application and is explicitly included in its composition. It may occupy one assembly or a related group of assemblies.
_Avoid_: Layer, plugin

**Plugin**:
An optional, trusted module discovered outside the application's explicit built-in composition. A plugin uses the same module entry point as a built-in module.
_Avoid_: Module

**Extension point**:
An intentional contract through which modules or plugins contribute to an application capability without depending on its implementation.
_Avoid_: Hook, internal API

**Contribution**:
A service or presentation model supplied by a module through an extension point.
_Avoid_: UI injection, patch
