using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using MiraiSpace.Presentation.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Access;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class WorkspaceMenuItemContainer : MenuItemViewModel, IAppMenuItemContainer
{
    private readonly SourceList<IAppMenuItem> _source = new();
    private readonly Lazy<WorkspacePageMenuItem> _pages;
    private readonly Lazy<WorkspaceCalendarMenuItem> _calendar;
    private readonly Lazy<IReadOnlyList<DelegateMenuItem>> _delegates;
    private readonly IAppMenuItemAccess _access;
    private readonly IComparer<IAppMenuItem> _comparer;
    private readonly ReadOnlyObservableCollection<IAppMenuItem> _items;
    private IDisposable? _itemsSubscription;

    public WorkspaceMenuItemContainer(
        AppNavigationState navigation,
        Lazy<WorkspacePageMenuItem> pages,
        Lazy<WorkspaceCalendarMenuItem> calendar,
        Lazy<IReadOnlyList<DelegateMenuItem>> delegates,
        IAppMenuItemAccess access,
        IComparer<IAppMenuItem> comparer)
        : base(navigation, 300)
    {
        _pages = pages;
        _calendar = calendar;
        _delegates = delegates;
        _access = access;
        _comparer = comparer;
        _itemsSubscription = _source.Connect()
            .Filter(_access.AccessChanged
                .Select(_ => new Func<IAppMenuItem, bool>(_access.CheckAccess))
                .StartWith(_access.CheckAccess))
            .Sort(_comparer)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Bind(out _items)
            .Subscribe();
    }

    public override string DisplayTitle => "Workspace";
    public override string Caption => "TEAM SPACE";
    public override string Glyph => "◇";
    public override string Accent => "#34A58B";
    public bool HasChildren => true;
    public IReadOnlyList<IAppMenuItem> Items => _items;

    protected override void OnActivated(CompositeDisposable disposables)
    {
        base.OnActivated(disposables);
        if (_source.Count == 0)
        {
            _source.AddRange([
                _pages.Value,
                _calendar.Value,
                .. _delegates.Value
            ]);
        }
    }

    protected override ValueTask ExecuteAsync(CancellationToken cancellationToken)
    {
        Navigation.Navigate(
            "WORKSPACE",
            "Your workspace",
            "Everything your team is building, planning, and sharing.",
            "#34A58B");
        return ValueTask.CompletedTask;
    }

    public override void Dispose()
    {
        _itemsSubscription?.Dispose();
        _itemsSubscription = null;
        _source.Dispose();
        base.Dispose();
    }
}
