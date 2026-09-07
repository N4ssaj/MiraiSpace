using MiraiSpace.Presentation.Navigation;

namespace MiraiSpace.Sample.Plugin;

public sealed class SampleRoute : NavigationRoute<SamplePage>
{
    public SampleRoute()
        : base()
    {
    }

    public override string Path { get; } = "/sample";
}
