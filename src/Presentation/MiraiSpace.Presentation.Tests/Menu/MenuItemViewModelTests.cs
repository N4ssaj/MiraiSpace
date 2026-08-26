using MiraiSpace.Presentation.Menu.Demo;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class MenuItemViewModelTests
{
    [Fact]
    public async Task SelectionTracksCurrentRouteAndParentRoute()
    {
        var navigation = new AppNavigationState();
        using var workspace = new TestMenuItem(navigation, "workspace");
        using var pages = new TestMenuItem(navigation, "workspace.pages");

        await pages.ExecuteAsync();

        Assert.True(workspace.IsSelected);
        Assert.True(pages.IsSelected);
    }

    private sealed class TestMenuItem(AppNavigationState navigation, string routeKey)
        : MenuItemViewModel(navigation, routeKey, 0)
    {
        public override string DisplayTitle => RouteKey;

        public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Navigation.Navigate(RouteKey, "TEST", RouteKey, string.Empty, "#000000");
            return ValueTask.CompletedTask;
        }
    }
}
