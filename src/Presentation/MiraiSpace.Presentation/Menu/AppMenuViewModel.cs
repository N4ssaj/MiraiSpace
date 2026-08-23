using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu;

public sealed class AppMenuViewModel : IAppMenuViewModel, IDisposable
{
    private readonly AppMenuItemCollection _itemCollection;
    private readonly IAppMenuItemExecutor _executor;
    private readonly ObservableCollection<IAppMenuItemContainer> _containers = [];
    private readonly ReadOnlyObservableCollection<IAppMenuItemContainer> _readOnlyContainers;

    public AppMenuViewModel(
        [Microsoft.Extensions.DependencyInjection.FromKeyedServices(AppMenuKeys.RootValue)]
        IEnumerable<IAppMenuItem> items,
        IAppMenuItemAccessChecker accessChecker,
        IAppMenuItemExecutor executor,
        IAppMenuScheduler scheduler)
    {
        _executor = executor;
        _itemCollection = new AppMenuItemCollection(items, accessChecker, scheduler);
        _readOnlyContainers = new ReadOnlyObservableCollection<IAppMenuItemContainer>(_containers);
        ((INotifyCollectionChanged)_itemCollection.Items).CollectionChanged += OnItemsChanged;
        RefreshContainers();
    }

    public ReadOnlyObservableCollection<IAppMenuItem> Items => _itemCollection.Items;

    public ReadOnlyObservableCollection<IAppMenuItemContainer> Containers => _readOnlyContainers;

    public ValueTask ExecuteAsync(
        IAppMenuItem item,
        CancellationToken cancellationToken = default) =>
        _executor.ExecuteAsync(item, cancellationToken);

    public void Dispose()
    {
        ((INotifyCollectionChanged)_itemCollection.Items).CollectionChanged -= OnItemsChanged;
        _itemCollection.Dispose();
    }

    private void OnItemsChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
        RefreshContainers();

    private void RefreshContainers()
    {
        _containers.Clear();
        foreach (IAppMenuItemContainer container in Items.OfType<IAppMenuItemContainer>())
        {
            _containers.Add(container);
        }
    }
}
