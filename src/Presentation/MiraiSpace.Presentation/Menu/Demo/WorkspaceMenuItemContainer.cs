using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class WorkspaceMenuItemContainer : MenuItemViewModel, IAppMenuItemContainer, IDisposable
{
    private readonly IAppMenuItem[] _availableItems;
    private readonly IAppMenuItemAccessChecker _accessChecker;
    private readonly IDisposable _accessChangedSubscription;

    public WorkspaceMenuItemContainer(
        AppNavigationState navigation,
        [FromKeyedServices(AppMenuKeys.Workspace)] IEnumerable<IAppMenuItem> registeredItems,
        IAppMenuItemAccessChecker accessChecker)
        : base(navigation, 300)
    {
        _accessChecker = accessChecker;
        _availableItems = registeredItems
            .Concat(CreateDelegateItems(navigation))
            .OrderBy(item => item.Order)
            .ToArray();
        Items = new ObservableCollection<IAppMenuItem>();
        RefreshItems();
        _accessChangedSubscription = accessChecker.AccessChanged.Subscribe(_ => RefreshItems());
    }

    public override string DisplayTitle => "Workspace";

    public override string Caption => "TEAM SPACE";

    public override string Glyph => "◇";

    public override string Accent => "#34A58B";

    public IList<IAppMenuItem> Items { get; }

    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Navigation.Navigate(
            "WORKSPACE",
            "Your workspace",
            "Everything your team is building, planning, and sharing.",
            "#34A58B");
        return ValueTask.CompletedTask;
    }

    public void Dispose() => _accessChangedSubscription.Dispose();

    private void RefreshItems()
    {
        Items.Clear();
        foreach (IAppMenuItem item in _availableItems.Where(_accessChecker.CheckAccess))
        {
            Items.Add(item);
        }
    }

    private static IEnumerable<IAppMenuItem> CreateDelegateItems(AppNavigationState navigation)
    {
        yield return new DelegateMenuItem(
            navigation,
            "Maya Chen",
            "MC",
            "#D87A5D",
            500);
        yield return new DelegateMenuItem(
            navigation,
            "Noah Wilson",
            "NW",
            "#557BC9",
            600);
    }
}
