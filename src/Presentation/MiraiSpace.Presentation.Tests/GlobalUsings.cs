global using Xunit;

using System.Reactive.Concurrency;
using System.Runtime.CompilerServices;
using ReactiveUI;

internal static class TestBootstrap
{
    [ModuleInitializer]
    internal static void Initialize() =>
        RxSchedulers.MainThreadScheduler = ImmediateScheduler.Instance;
}
