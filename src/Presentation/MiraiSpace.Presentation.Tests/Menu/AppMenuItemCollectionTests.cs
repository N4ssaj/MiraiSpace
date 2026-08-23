using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuItemCollectionTests
{
    [Fact]
    public void ItemsAreSortedByOrderThenId()
    {
        using var collection = CreateCollection(
            new TestMenuItem("z", 100),
            new TestMenuItem("a", 100),
            new TestMenuItem("last", 200));

        Assert.Equal(["a", "z", "last"], collection.Items.Select(x => x.Id));
    }

    [Fact]
    public void DuplicateIdsFailFast()
    {
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
            CreateCollection(new TestMenuItem("duplicate", 100), new TestMenuItem("duplicate", 200)));

        Assert.Contains("duplicate", exception.Message);
    }

    [Fact]
    public void AccessInvalidationRefiltersExistingItems()
    {
        var policy = new ToggleAccessPolicy(allowed: false);
        var checker = new AppMenuItemAccessChecker([policy]);
        using var collection = new AppMenuItemCollection(
            [new RestrictedTestMenuItem("restricted", 100)],
            checker,
            ImmediateMenuScheduler);

        Assert.Empty(collection.Items);

        policy.SetAllowed(true);

        Assert.Equal("restricted", Assert.Single(collection.Items).Id);
    }

    [Fact]
    public void OwnedItemsAreDisposedWhenRemoved()
    {
        using var collection = CreateCollection();
        var item = new DisposableTestMenuItem("owned", 100);
        collection.AddOwned([item]);

        Assert.True(collection.Remove(item.Id));
        Assert.True(item.IsDisposed);
    }

    [Fact]
    public void RemainingOwnedItemsAreDisposedWithCollection()
    {
        var collection = CreateCollection();
        var item = new DisposableTestMenuItem("owned", 100);
        collection.AddOwned([item]);

        collection.Dispose();

        Assert.True(item.IsDisposed);
    }

    private static AppMenuItemCollection CreateCollection(params IAppMenuItem[] items) =>
        new(
            items,
            new AppMenuItemAccessChecker([]),
            ImmediateMenuScheduler);

    private static IAppMenuScheduler ImmediateMenuScheduler { get; } =
        new AppMenuScheduler(ImmediateScheduler.Instance);

    private sealed class ToggleAccessPolicy(bool allowed) : IAppMenuItemAccessPolicy, IDisposable
    {
        private readonly Subject<Unit> _changed = new();
        private bool _allowed = allowed;

        public IObservable<Unit> AccessChanged => _changed;

        public bool AppliesTo(IAppMenuItem item) => item is RestrictedTestMenuItem;

        public bool CheckAccess(IAppMenuItem item) => _allowed;

        public void SetAllowed(bool allowed)
        {
            _allowed = allowed;
            _changed.OnNext(Unit.Default);
        }

        public void Dispose() => _changed.Dispose();
    }

    private class TestMenuItem(string id, int order) : IAppMenuItem
    {
        public string Id { get; } = id;

        public int Order { get; } = order;

        public ValueTask ExecuteAsync(CancellationToken cancellationToken = default) =>
            ValueTask.CompletedTask;
    }

    private sealed class RestrictedTestMenuItem(string id, int order) : TestMenuItem(id, order);

    private sealed class DisposableTestMenuItem(string id, int order) : TestMenuItem(id, order), IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose() => IsDisposed = true;
    }
}
