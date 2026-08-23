using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using DynamicData.Kernel;
using MiraiSpace.Extensibility.Abstractions.Menu;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu;

public sealed class AppMenuItemCollection : IDisposable
{
    private readonly SourceCache<IAppMenuItem, string> _itemCache = new(x => x.Id);
    private readonly HashSet<string> _ownedItemIds = [];
    private readonly IDisposable _subscription;
    private readonly ReadOnlyObservableCollection<IAppMenuItem> _items;
    private bool _isDisposed;

    public AppMenuItemCollection(
        IEnumerable<IAppMenuItem> items,
        IAppMenuItemAccessChecker accessChecker,
        IComparer<IAppMenuItem>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(accessChecker);

        IAppMenuItem[] initialItems = items.ToArray();
        ThrowIfDuplicateIds(initialItems, checkExistingItems: false);

        _subscription = _itemCache
            .Connect()
            .Filter(
                accessChecker.AccessChanged
                    .Select(_ => new Func<IAppMenuItem, bool>(accessChecker.CheckAccess))
                    .StartWith(accessChecker.CheckAccess))
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .SortAndBind(
                out _items,
                comparer ?? AppMenuItemComparers.Default,
                new SortAndBindOptions { UseReplaceForUpdates = true })
            .Subscribe();

        _itemCache.AddOrUpdate(initialItems);
    }

    public ReadOnlyObservableCollection<IAppMenuItem> Items => _items;

    public void AddBorrowed(IEnumerable<IAppMenuItem> items) => Add(items, owned: false);

    public void AddOwned(IEnumerable<IAppMenuItem> items) => Add(items, owned: true);

    public bool Remove(string id)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        Optional<IAppMenuItem> item = _itemCache.Lookup(id);
        if (!item.HasValue)
        {
            return false;
        }

        _itemCache.RemoveKey(id);
        if (_ownedItemIds.Remove(id) && item.Value is IDisposable disposable)
        {
            disposable.Dispose();
        }

        return true;
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        foreach (string id in _ownedItemIds.ToArray())
        {
            Optional<IAppMenuItem> item = _itemCache.Lookup(id);
            if (item.HasValue && item.Value is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        _ownedItemIds.Clear();
        _subscription.Dispose();
        _itemCache.Dispose();
    }

    private void Add(IEnumerable<IAppMenuItem> items, bool owned)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
        IAppMenuItem[] materializedItems = items.ToArray();

        ThrowIfDuplicateIds(materializedItems, checkExistingItems: true);

        _itemCache.AddOrUpdate(materializedItems);
        if (owned)
        {
            _ownedItemIds.UnionWith(materializedItems.Select(x => x.Id));
        }
    }

    private void ThrowIfDuplicateIds(
        IEnumerable<IAppMenuItem> items,
        bool checkExistingItems)
    {
        string[] duplicateIds = items
            .GroupBy(x => x.Id)
            .Where(x => x.Count() > 1
                || checkExistingItems && _itemCache.Lookup(x.Key).HasValue)
            .Select(x => x.Key)
            .Order()
            .ToArray();

        if (duplicateIds.Length > 0)
        {
            throw new InvalidOperationException(
                $"Duplicate application menu item IDs: {string.Join(", ", duplicateIds)}.");
        }

    }
}
