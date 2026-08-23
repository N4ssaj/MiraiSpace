using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu;

public sealed class AppMenuViewModel : IAppMenuViewModel, IDisposable
{
    private readonly SourceCache<IAppMenuItem, string> _itemCache = new(x => x.Id);
    private readonly IAppMenuItemAccessChecker _accessChecker;
    private readonly IDisposable _subscription;
    private readonly ReadOnlyObservableCollection<IAppMenuItem> _items;

    public AppMenuViewModel(
        [Microsoft.Extensions.DependencyInjection.FromKeyedServices(AppMenuKeys.RootValue)]
        IEnumerable<IAppMenuItem> items,
        IAppMenuItemAccessChecker accessChecker)
    {
        _accessChecker = accessChecker;
        _subscription = _itemCache
            .Connect()
            .Filter(
                accessChecker.AccessChanged
                    .Select(_ => new Func<IAppMenuItem, bool>(accessChecker.CheckAccess))
                    .StartWith(accessChecker.CheckAccess))
            .SortAndBind(
                out _items,
                AppMenuItemComparers.Default,
                new SortAndBindOptions
                {
                    UseReplaceForUpdates = true
                })
            .Subscribe();

        AddUnique(items);
    }

    public ReadOnlyObservableCollection<IAppMenuItem> Items => _items;

    public ValueTask ExecuteAsync(
        IAppMenuItem item,
        CancellationToken cancellationToken = default) =>
        _accessChecker.CheckAccess(item)
            ? item.ExecuteAsync(cancellationToken)
            : ValueTask.CompletedTask;

    public void Dispose()
    {
        _subscription.Dispose();
        _itemCache.Dispose();
    }

    private void AddUnique(IEnumerable<IAppMenuItem> items)
    {
        IAppMenuItem[] materializedItems = items.ToArray();
        string[] duplicateIds = materializedItems
            .GroupBy(x => x.Id)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .ToArray();

        if (duplicateIds.Length > 0)
        {
            throw new InvalidOperationException(
                $"Duplicate application menu item IDs: {string.Join(", ", duplicateIds)}.");
        }

        _itemCache.AddOrUpdate(materializedItems);
    }
}
