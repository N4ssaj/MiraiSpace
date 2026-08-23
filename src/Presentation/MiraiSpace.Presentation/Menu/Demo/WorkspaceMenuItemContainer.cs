using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class WorkspaceMenuItemContainer : MenuItemViewModel, IAppMenuItemContainer, IDisposable
{
    private readonly SourceCache<IAppMenuItem, string> _itemCache = new(x => x.Id);
    private readonly IDisposable _subscription;
    private readonly ReadOnlyObservableCollection<IAppMenuItem> _items;

    public WorkspaceMenuItemContainer(
        AppNavigationState navigation,
        [FromKeyedServices(AppMenuKeys.WorkspaceValue)] IEnumerable<IAppMenuItem> registeredItems,
        IAppMenuItemAccessChecker accessChecker)
        : base(navigation, "core.workspace", 300)
    {
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

        List<IAppMenuItem> allItems = [.. registeredItems, .. CreateDelegateItems(navigation)];
        EnsureUniqueIds(allItems);
        _itemCache.AddOrUpdate(allItems);
    }

    public string Title => "Workspace";

    public override string DisplayTitle => Title;

    public override string Caption => "TEAM SPACE";

    public override string Glyph => "◇";

    public override string Accent => "#34A58B";

    public ReadOnlyObservableCollection<IAppMenuItem> Items => _items;

    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Navigation.Navigate(
            "WORKSPACE",
            "Your workspace",
            "Everything your team is building, planning, and sharing.",
            "#34A58B");
        return ValueTask.CompletedTask;
    }

    public void Dispose()
    {
        _subscription.Dispose();
        _itemCache.Dispose();
    }

    private static IEnumerable<IAppMenuItem> CreateDelegateItems(AppNavigationState navigation)
    {
        yield return new DelegateMenuItem(
            navigation,
            Guid.Parse("9d5a33c5-bd05-4ead-a2fc-fdb6d483e978"),
            "Maya Chen",
            "MC",
            "#D87A5D",
            500);
        yield return new DelegateMenuItem(
            navigation,
            Guid.Parse("611b88fb-c25e-42be-a8a5-d6fe9f2d630f"),
            "Noah Wilson",
            "NW",
            "#557BC9",
            600);
    }

    private static void EnsureUniqueIds(IEnumerable<IAppMenuItem> items)
    {
        string[] duplicates = items
            .GroupBy(x => x.Id)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .ToArray();

        if (duplicates.Length > 0)
        {
            throw new InvalidOperationException(
                $"Duplicate workspace menu item IDs: {string.Join(", ", duplicates)}.");
        }
    }
}
