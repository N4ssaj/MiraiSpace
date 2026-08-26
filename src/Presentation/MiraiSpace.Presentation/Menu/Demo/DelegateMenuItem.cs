using MiraiSpace.Presentation.Menu.Standard;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class DelegateMenuItem(
    AppNavigationState navigation,
    string displayName,
    string initials,
    string color,
    int order) : StandardAppMenuItem(order), IWorkspaceMenuItem
{
    public override string Title { get; } = displayName;
    public override string Caption => "Delegated space";
    public override string Glyph { get; } = initials;
    public override string Accent { get; } = color;

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        navigation.Navigate("DELEGATED SPACE", Title, $"You are viewing the pages delegated by {Title}.", Accent);
        return Task.CompletedTask;
    }
}
