using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class DashboardMenuItem(AppNavigationState navigation)
    : DemoMenuContribution(navigation, new(
        "overview", null, 100, "Overview", "Your daily pulse", "⌂"))
{
    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default) =>
        NavigateAsync(
            "OVERVIEW",
            "Good morning, Alex",
            "Here is what is happening across your workspace today.");
}
