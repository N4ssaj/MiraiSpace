using System.Reactive;
using System.Reactive.Subjects;
using MiraiSpace.Presentation.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Access;
using MiraiSpace.Presentation.Menu.Composition;
using MiraiSpace.Presentation.Menu.Demo;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuTests
{
    [Fact]
    public void OptionalOrderIsAppliedByTheOwnerComparer()
    {
        using var last = new DashboardMenuItem(new AppNavigationState());
        using var first = new TestItem(order: 10);
        using var menu = CreateMenu(last, first);

        Assert.Same(first, menu.Items[0]);
        Assert.Same(last, menu.Items[1]);
    }

    [Fact]
    public void AccessChangeRefiltersWithoutReactiveContractsInAbstractions()
    {
        using var item = new TestItem(order: 10);
        using var policy = new ToggleAccessPolicy(allowed: false);
        using var menu = CreateMenu([item], policy);

        Assert.Empty(menu.Items);

        policy.SetAllowed(true);

        Assert.Same(item, Assert.Single(menu.Items));
    }

    private static AppMenu CreateMenu(params IAppMenuItem[] items) =>
        CreateMenu(items, new ToggleAccessPolicy(allowed: true));

    private static AppMenu CreateMenu(
        IEnumerable<IAppMenuItem> items,
        IAppMenuItemAccessPolicy policy)
    {
        var access = new AppMenuItemAccess([policy]);
        return new AppMenu(items, access, new AppMenuItemComparer());
    }

    private sealed class TestItem(int order) : MiraiSpace.Presentation.Menu.Items.AppMenuItem(order)
    {
        protected override ValueTask ExecuteAsync(CancellationToken cancellationToken) =>
            ValueTask.CompletedTask;
    }

    private sealed class ToggleAccessPolicy(bool allowed) : IAppMenuItemAccessPolicy, IDisposable
    {
        private readonly Subject<Unit> _changed = new();
        private bool _allowed = allowed;

        public IObservable<Unit> AccessChanged => _changed;
        public bool CheckAccess(IAppMenuItem item) => _allowed;

        public void SetAllowed(bool allowed)
        {
            _allowed = allowed;
            _changed.OnNext(Unit.Default);
        }

        public void Dispose() => _changed.Dispose();
    }
}
