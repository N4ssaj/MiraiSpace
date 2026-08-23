using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public sealed class WorkspaceMenuItemContainer : MenuItemViewModel, IAppMenuItemContainer, IDisposable
{
    private readonly AppMenuItemCollection _itemCollection;

    public WorkspaceMenuItemContainer(
        AppNavigationState navigation,
        [FromKeyedServices(AppMenuKeys.WorkspaceValue)] IEnumerable<IAppMenuItem> registeredItems,
        IAppMenuItemAccessChecker accessChecker,
        IAppMenuScheduler scheduler)
        : base(navigation, "core.workspace", 300)
    {
        _itemCollection = new AppMenuItemCollection(registeredItems, accessChecker, scheduler);
        _itemCollection.AddOwned(CreateDelegateItems(navigation));
    }

    public string Title => "Workspace";

    public override string DisplayTitle => Title;

    public override string Caption => "TEAM SPACE";

    public override string Glyph => "◇";

    public override string Accent => "#34A58B";

    public ReadOnlyObservableCollection<IAppMenuItem> Items => _itemCollection.Items;

    public override ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Navigation.Navigate(
            "WORKSPACE",
            "Your workspace",
            "Everything your team is building, planning, and sharing.",
            "#34A58B");
        return ValueTask.CompletedTask;
    }

    public void Dispose() => _itemCollection.Dispose();

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
}
