using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.ViewModels;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu;

public sealed class AppMenuViewModel : ViewModelBase, IAppMenuViewModel
{
    private readonly SourceList<IAppMenuItem> _itemSource = new();
    private readonly IAppMenuItemExecutor _executor;
    private readonly ReadOnlyObservableCollection<IAppMenuItem> _items;

    public AppMenuViewModel(
        [Microsoft.Extensions.DependencyInjection.FromKeyedServices(AppMenuKeys.Root)]
        IEnumerable<IAppMenuItem> items,
        IAppMenuItemAccessChecker accessChecker,
        IAppMenuItemExecutor executor)
    {
        _executor = executor;
        IComparer<IAppMenuItem> comparer = SortExpressionComparer<IAppMenuItem>
            .Ascending(item => item.Order);

        Own(_itemSource
            .Connect()
            .Filter(
                accessChecker.AccessChanged
                    .Select(_ => new Func<IAppMenuItem, bool>(accessChecker.CheckAccess))
                    .StartWith(accessChecker.CheckAccess))
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Sort(comparer)
            .Bind(out _items)
            .Subscribe());
        Own(_itemSource);

        _itemSource.AddRange(items);
    }

    public IReadOnlyList<IAppMenuItem> Items => _items;

    public ValueTask ExecuteAsync(
        IAppMenuItem item,
        CancellationToken cancellationToken = default) =>
        _executor.ExecuteAsync(item, cancellationToken);
}
