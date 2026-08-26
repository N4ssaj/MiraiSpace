using System.ComponentModel;
using System.Reactive.Disposables;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.ViewModels;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Demo;

public abstract class MenuItemViewModel : ViewModelBase, IAppMenuItem
{
    protected MenuItemViewModel(AppNavigationState navigation, string routeKey, int order)
    {
        Navigation = navigation;
        RouteKey = routeKey;
        Order = order;
        Navigation.PropertyChanged += OnNavigationPropertyChanged;
        Own(Disposable.Create(() => Navigation.PropertyChanged -= OnNavigationPropertyChanged));
    }

    protected AppNavigationState Navigation { get; }

    public string RouteKey { get; }

    public bool IsSelected => Navigation.RouteKey == RouteKey
        || Navigation.RouteKey.StartsWith($"{RouteKey}.", StringComparison.Ordinal);

    public int Order { get; }

    public virtual IEnumerable<MenuItemViewModel> Children => [];

    public abstract string DisplayTitle { get; }

    public virtual string Caption => string.Empty;

    public virtual string Glyph => "•";

    public virtual string Accent => "#7165E8";

    public virtual string Badge => string.Empty;

    public bool HasBadge => !string.IsNullOrWhiteSpace(Badge);

    public abstract ValueTask ExecuteAsync(CancellationToken cancellationToken = default);

    private void OnNavigationPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(AppNavigationState.RouteKey))
        {
            this.RaisePropertyChanged(nameof(IsSelected));
        }
    }
}
