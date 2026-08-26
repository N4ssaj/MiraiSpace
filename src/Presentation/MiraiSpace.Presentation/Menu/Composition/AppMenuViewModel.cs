using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using MiraiSpace.Presentation.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Access;
using MiraiSpace.Presentation.ViewModels;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Composition;

public sealed class AppMenuViewModel : Component, IAppMenu
{
    private readonly SourceList<IAppMenuItem> _source = new();
    private readonly IObservable<IChangeSet<IAppMenuItem>> _pipeline;
    private readonly ReadOnlyObservableCollection<IAppMenuItem> _items;

    public AppMenuViewModel(
        IEnumerable<IAppMenuItem> items,
        AppMenuAccessEvaluator access,
        IAppMenuItemComparer comparer)
    {
        _pipeline = _source
            .Connect()
            .Filter(access.AccessChanged
                .Select(_ => new Func<IAppMenuItem, bool>(access.CheckAccess))
                .StartWith(access.CheckAccess))
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Sort(comparer)
            .Bind(out _items);
        _source.AddRange(items);
    }

    public IReadOnlyList<IAppMenuItem> Items => _items;

    protected override void OnActivated(CompositeDisposable disposables)
    {
        disposables.Add(_pipeline.Subscribe());
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _source.Dispose();
        base.Dispose(disposing);
    }
}
