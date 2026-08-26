using System.Runtime.CompilerServices;
using ReactiveUI.Builder;

namespace MiraiSpace.Presentation.Tests;

internal static class ReactiveUiTestBootstrap
{
    [ModuleInitializer]
    internal static void Initialize() =>
        RxAppBuilder.CreateReactiveUIBuilder()
            .WithCoreServices()
            .BuildApp();
}
