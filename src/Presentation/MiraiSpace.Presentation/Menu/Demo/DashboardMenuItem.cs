namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class DashboardMenuItem(AppNavigationState navigation)
    : MenuItemViewModel(navigation, 100)
{
    public string Title => "Overview";

    public string Subtitle => "Your daily pulse";

    public override string DisplayTitle => Title;

    public override string Caption => Subtitle;

    public override string Glyph => "⌂";

    public ValueTask ExecuteAsyncCore()
    {
        Navigation.Navigate(
            "OVERVIEW",
            "Good morning, Alex",
            "Here is what is happening across your workspace today.",
            "#7165E8");
        return ValueTask.CompletedTask;
    }

    protected override ValueTask ExecuteAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsyncCore();
}
