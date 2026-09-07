using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Foundation;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu;

public sealed class AppMenuViewModel : ReactiveComponent, IDisposable
{
    private readonly ReadOnlyObservableCollection<IAppMenuItem> _items;
    private readonly IDisposable _binding;
    private readonly SourceList<IAppMenuItem> _itemSource = new();
    private readonly IReadOnlyList<IAppMenuAccessPolicy> _policies;

    public ReadOnlyObservableCollection<IAppMenuItem> Items => _items;

    public AppMenuViewModel(
        [FromKeyedServices(AppMenuKeys.Root)]
        IEnumerable<IAppMenuItem> items,
        IEnumerable<IAppMenuAccessPolicy> policies)
    {
        _policies = [.. policies];
        _binding = _itemSource
            .Connect()
            .Filter(
                ObserveAccessInvalidations(),
                (_, item) => CanAccess(item),
                ListFilterPolicy.ClearAndReplace)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Bind(
                out _items,
                new BindingOptions(
                    BindingOptions.DefaultResetThreshold,
                    UseReplaceForUpdates: true))
            .Subscribe();
        _itemSource.AddRange(items);
    }

    public void Dispose()
    {
        _binding.Dispose();
        _itemSource.Dispose();
    }

    private IObservable<Unit> ObserveAccessInvalidations() =>
        _policies
            .Select(policy => policy.Invalidated)
            .Merge()
            .StartWith(Unit.Default);

    private bool CanAccess(IAppMenuItem item) =>
        _policies.All(policy => policy.CanAccess(item));

}
