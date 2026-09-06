global using Xunit;

using System.Reactive.Concurrency;
using System.Runtime.CompilerServices;
using ReactiveUI;
using ReactiveUI.Builder;

internal static class TestBootstrap
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        RxAppBuilder
            .CreateReactiveUIBuilder()
            .WithCoreServices()
            .BuildApp();

        RxSchedulers.MainThreadScheduler = ImmediateScheduler.Instance;
    }
}
