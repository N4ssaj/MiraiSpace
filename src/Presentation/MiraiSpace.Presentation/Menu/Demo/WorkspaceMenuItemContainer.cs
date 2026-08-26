using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using MiraiSpace.Presentation.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Access;
using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class WorkspaceMenuItemContainer : StandardAppMenuItem, IAppMenuItemContainer
{
    private readonly SourceList<IAppMenuItem> _source = new();
    private readonly IObservable<IChangeSet<IAppMenuItem>> _pipeline;
    private readonly IDisposable _subscription;
    private readonly ReadOnlyObservableCollection<IAppMenuItem> _items;
    private readonly AppNavigationState _navigation;

    public WorkspaceMenuItemContainer(
        AppNavigationState navigation,
        IEnumerable<IWorkspaceMenuItem> items,
        AppMenuAccessEvaluator access,
        IAppMenuItemComparer comparer) : base(300)
    {
        _navigation = navigation;
        _pipeline = _source
            .Connect()
            .Filter(access.AccessChanged
                .Select(_ => new Func<IAppMenuItem, bool>(access.CheckAccess))
                .StartWith(access.CheckAccess))
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Sort(comparer)
            .Bind(out _items);
        _source.AddRange(items.Cast<IAppMenuItem>());
        _subscription = _pipeline.Subscribe();
    }

    public override string Title => "Workspace";
    public override string Caption => "Team space";
    public override string Glyph => "◇";
    public override string Accent => "#34A58B";
    public IReadOnlyList<IAppMenuItem> Items => _items;

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _navigation.Navigate("WORKSPACE", "Your workspace", "Everything your team is building, planning, and sharing.", Accent);
        return Task.CompletedTask;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _subscription.Dispose();
            _source.Dispose();
        }
        base.Dispose(disposing);
    }
}
