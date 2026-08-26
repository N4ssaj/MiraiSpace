using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Access;
using MiraiSpace.Presentation.ViewModels;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Composition;

public sealed class AppMenu : Component, IAppMenu
{
    private readonly SourceList<IAppMenuItem> _source = new();
    private readonly IAppMenuItemAccess _access;
    private readonly IDisposable _itemsSubscription;
    private readonly ReadOnlyObservableCollection<IAppMenuItem> _items;

    public AppMenu(
        [FromKeyedServices(AppMenuKeys.Root)] IEnumerable<IAppMenuItem> items,
        IAppMenuItemAccess access,
        IComparer<IAppMenuItem> comparer)
    {
        _access = access;
        _itemsSubscription = _source.Connect()
            .Filter(_access.AccessChanged
                .Select(_ => new Func<IAppMenuItem, bool>(_access.CheckAccess))
                .StartWith(_access.CheckAccess))
            .Sort(comparer)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Bind(out _items)
            .Subscribe();
        _source.AddRange(items);
    }

    public IReadOnlyList<IAppMenuItem> Items => _items;

    public override void Dispose()
    {
        _itemsSubscription.Dispose();
        _source.Dispose();
        base.Dispose();
    }
}
