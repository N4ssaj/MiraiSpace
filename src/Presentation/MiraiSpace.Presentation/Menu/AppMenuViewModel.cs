using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Foundation;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu;

public sealed class AppMenuViewModel : ReactiveComponent
{
    private readonly ReadOnlyObservableCollection<IAppMenuItem> _items;
    private readonly IObservable<IChangeSet<IAppMenuItem>> _itemsPipeline;
    private readonly SourceList<IAppMenuItem> _itemSource = new();
    private readonly IReadOnlyList<IAppMenuAccessPolicy> _policies;

    public ReadOnlyObservableCollection<IAppMenuItem> Items => _items;

    public AppMenuViewModel(
        [FromKeyedServices(AppMenuKeys.Root)]
        IEnumerable<IAppMenuItem> items,
        IEnumerable<IAppMenuAccessPolicy> policies)
    {
        _policies = [.. policies];
        var itemsPipeline = _itemSource
            .Connect()
            .Filter(
                ObserveAccessInvalidations(),
                (_, item) => CanAccess(item),
                ListFilterPolicy.ClearAndReplace);

        _itemsPipeline = ResetBeforeRebinding(itemsPipeline)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Bind(
                out _items,
                new BindingOptions(
                    BindingOptions.DefaultResetThreshold,
                    UseReplaceForUpdates: true));
        _itemSource.AddRange(items);
    }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        _itemsPipeline
            .Subscribe()
            .DisposeWith(disposables);
    }

    private IObservable<Unit> ObserveAccessInvalidations() =>
        _policies
            .Select(policy => policy.Invalidated)
            .Merge()
            .StartWith(Unit.Default);

    private bool CanAccess(IAppMenuItem item) =>
        _policies.All(policy => policy.CanAccess(item));

    private IObservable<IChangeSet<IAppMenuItem>> ResetBeforeRebinding(
        IObservable<IChangeSet<IAppMenuItem>> source) =>
        Observable.Defer(() =>
        {
            if (_items.Count == 0)
            {
                return source;
            }

            var reset = new ChangeSet<IAppMenuItem>(
                [new Change<IAppMenuItem>(ListChangeReason.Clear, _items.ToArray())]);
            return Observable.Return(reset).Concat(source);
        });
}
