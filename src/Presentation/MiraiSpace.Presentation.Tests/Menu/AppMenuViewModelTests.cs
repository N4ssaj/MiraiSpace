using System.Reactive;
using System.Reactive.Subjects;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuViewModelTests
{
    [Fact]
    public void ItemsAreSortedByOrder()
    {
        using var menu = CreateMenu(
            new TestMenuItem("last", 20),
            new TestMenuItem("first", 10));

        Assert.Equal(["first", "last"], menu.Items.Cast<TestMenuItem>().Select(item => item.Name));
    }

    [Fact]
    public void AccessChangeRefiltersItems()
    {
        var policy = new ToggleAccessPolicy(allowed: false);
        using var menu = new AppMenuViewModel(
            [new TestMenuItem("restricted", 10)],
            new AppMenuItemAccessChecker([policy]),
            new AppMenuItemExecutor(new AppMenuItemAccessChecker([policy])));

        Assert.Empty(menu.Items);

        policy.SetAllowed(true);

        Assert.Equal("restricted", Assert.IsType<TestMenuItem>(Assert.Single(menu.Items)).Name);
    }

    private static AppMenuViewModel CreateMenu(params IAppMenuItem[] items)
    {
        var checker = new AppMenuItemAccessChecker([]);
        return new AppMenuViewModel(items, checker, new AppMenuItemExecutor(checker));
    }

    private sealed class ToggleAccessPolicy(bool allowed) : IAppMenuItemAccessPolicy, IDisposable
    {
        private readonly Subject<Unit> _accessChanged = new();
        private bool _allowed = allowed;

        public IObservable<Unit> AccessChanged => _accessChanged;

        public bool CheckAccess(IAppMenuItem item) => _allowed;

        public void SetAllowed(bool allowed)
        {
            _allowed = allowed;
            _accessChanged.OnNext(Unit.Default);
        }

        public void Dispose() => _accessChanged.Dispose();
    }

    private sealed class TestMenuItem(string name, int order) : IAppMenuItem
    {
        public string Name { get; } = name;

        public int Order { get; } = order;

        public ValueTask ExecuteAsync(CancellationToken cancellationToken = default) =>
            ValueTask.CompletedTask;
    }
}
