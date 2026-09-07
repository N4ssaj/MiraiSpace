# Official sources

Use current primary documentation when an API or recommendation may have changed:

- [Avalonia documentation](https://docs.avaloniaui.net/)
- [Avalonia compiled bindings](https://docs.avaloniaui.net/docs/data-binding/compiled-bindings)
- [Avalonia ReactiveUI integration](https://docs.avaloniaui.net/docs/concepts/reactiveui/)
- [ReactiveUI documentation](https://www.reactiveui.net/docs/)
- [ReactiveUI repository](https://github.com/reactiveui/ReactiveUI)
- [.NET dependency injection guidelines](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines)
- [.NET application architecture guides](https://learn.microsoft.com/en-us/dotnet/architecture/)
- [MVVM architecture guidance](https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm)
- [.NET asynchronous programming](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/)
- [.NET testing guidance](https://learn.microsoft.com/en-us/dotnet/core/testing/)

The MAUI architecture pages describe general client patterns; translate platform APIs to Avalonia rather than copying MAUI-specific types.

## MiraiSpace package research

- [MessagePipe](https://github.com/Cysharp/MessagePipe)
- [Xaml.Behaviors by wieslawsoltes](https://github.com/wieslawsoltes/Xaml.Behaviors)
- [Serilog Generic Host integration](https://github.com/serilog/serilog-extensions-hosting)
- [Extensions.Hosting](https://github.com/reactivemarbles/Extensions.Hosting)
- [Autofac lifetime scopes](https://autofac.readthedocs.io/en/latest/lifetime/working-with-scopes.html)
- [Autofac .NET integration](https://autofac.readthedocs.io/en/latest/integration/netcore.html)
- [ReactiveUI.Validation](https://github.com/reactiveui/ReactiveUI.Validation)

Check `src/Directory.Packages.props` and installed package documentation before suggesting exact API syntax. Use Context7 as directed by user/repository instructions, then primary sources where coverage is missing. Distinguish current ReactiveUI exception bootstrap APIs from older `RxApp` examples. These are research references, not automatically approved dependencies; see [API agreements](../../../../docs/architecture/api-agreements.md).
- [ReactiveUI.Binding.SourceGenerators](https://github.com/reactiveui/ReactiveUI.Binding.SourceGenerators): MiraiSpace currently pins runtime and generator 3.4.0 to preserve Splat 19.4.1. Inspect generated call sites; package installation alone does not migrate legacy WhenAnyValue calls. Version 3.4.0 packages the analyzer separately despite its README. See the repository library review before changing this integration.
- [MPowerKit.Navigation](https://github.com/MPowerKit/Navigation): navigation/lifetime reference; do not copy MAUI-specific ownership into Avalonia.
- [HanumanInstitute.MvvmDialogs](https://github.com/mysteryx93/HanumanInstitute.MvvmDialogs): dialog-host reference for discussion after navigation.
