using MiraiSpace.Presentation.Menu.Standard;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class DashboardMenuItem(AppNavigationState navigation) : StandardAppMenuItem(100)
{
    public override string Title => "Overview";
    public override string Caption => "Your daily pulse";
    public override string Glyph => "⌂";

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        navigation.Navigate("OVERVIEW", "Good morning, Alex", "Here is what is happening across your workspace today.", Accent);
        return Task.CompletedTask;
    }
}
