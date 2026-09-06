using System.Reactive;
using System.Reactive.Subjects;
using System.Windows.Input;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Features.Workspace.Menu;
using MiraiSpace.Presentation.Menu;
using ReactiveUI;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuViewModelTests
{
    [Fact]
    public void ActivationPreservesRegistrationOrder()
    {
        var first = new TestMenuItem();
        var second = new TestMenuItem();
        var menu = new AppMenuViewModel([first, second], []);

        using var activation = menu.Activator.Activate();

        Assert.Collection(
            menu.Items,
            item => Assert.Same(first, item),
            item => Assert.Same(second, item));
    }

    [Fact]
    public void PolicyInvalidationRefiltersItems()
    {
        using var policy = new ToggleAccessPolicy(false);
        var item = new TestMenuItem();
        var menu = new AppMenuViewModel([item], [policy]);
        using var activation = menu.Activator.Activate();

        Assert.Empty(menu.Items);

        policy.SetAccess(true);

        Assert.Same(item, Assert.Single(menu.Items));
    }

    [Fact]
    public void ReactivationDoesNotDuplicateItems()
    {
        var item = new TestMenuItem();
        var menu = new AppMenuViewModel([item], []);

        menu.Activator.Activate().Dispose();
        using var activation = menu.Activator.Activate();

        Assert.Same(item, Assert.Single(menu.Items));
    }

    [Fact]
    public void ContainerReactivationDoesNotDuplicateItems()
    {
        var item = new TestMenuItem();
        var container = new WorkspaceMenuItemContainer([item], []);

        container.Activator.Activate().Dispose();
        using var activation = container.Activator.Activate();

        Assert.Same(item, Assert.Single(container.Items));
    }

    private sealed class ToggleAccessPolicy : IAppMenuAccessPolicy, IDisposable
    {
        private readonly Subject<Unit> _invalidated = new();
        private bool _canAccess;

        public IObservable<Unit> Invalidated => _invalidated;

        public ToggleAccessPolicy(bool canAccess)
        {
            _canAccess = canAccess;
        }

        public bool CanAccess(IAppMenuItem item) => _canAccess;

        public void SetAccess(bool canAccess)
        {
            _canAccess = canAccess;
            _invalidated.OnNext(Unit.Default);
        }

        public void Dispose() => _invalidated.Dispose();
    }

    private sealed class TestMenuItem : IAppMenuItem
    {
        public ICommand ExecuteCommand { get; } =
            ReactiveCommand.Create(() => { });
    }
}
