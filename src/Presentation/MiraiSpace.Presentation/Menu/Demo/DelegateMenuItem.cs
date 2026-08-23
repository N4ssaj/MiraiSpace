namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class DelegateMenuItem(
    AppNavigationState navigation,
    Guid delegateId,
    string displayName,
    string initials,
    string color,
    int order)
    : MenuItemViewModel(navigation, $"delegate:{delegateId:N}", order)
{
    public string DisplayName { get; } = displayName;

    public string Initials { get; } = initials;

    public string Color { get; } = color;

    public override string DisplayTitle => DisplayName;

    public override string Caption => "Delegated pages";

    public override string Glyph => Initials;

    public override string Accent => Color;

    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Navigation.Navigate(
            "DELEGATED SPACE",
            DisplayName,
            $"You are viewing the pages delegated by {DisplayName}.",
            Color);
        return ValueTask.CompletedTask;
    }
}
