using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class DelegateMenuItem(
    AppNavigationState navigation,
    string displayName,
    string initials,
    string color,
    int order)
    : DemoMenuContribution(navigation, new(
        $"workspace.delegate.{initials.ToLowerInvariant()}",
        "workspace",
        order,
        displayName,
        "Delegated space",
        initials,
        color))
{
    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default) =>
        NavigateAsync(
            "DELEGATED SPACE",
            Descriptor.Title,
            $"You are viewing the pages delegated by {Descriptor.Title}.");
}
