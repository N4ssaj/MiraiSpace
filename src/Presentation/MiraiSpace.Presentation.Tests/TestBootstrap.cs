using System.Runtime.CompilerServices;
using ReactiveUI.Builder;

namespace MiraiSpace.Presentation.Tests;

public static class TestBootstrap
{
    [ModuleInitializer]
    public static void InitializeReactiveUi() =>
        RxAppBuilder.CreateReactiveUIBuilder()
            .WithCoreServices()
            .BuildApp();
}
