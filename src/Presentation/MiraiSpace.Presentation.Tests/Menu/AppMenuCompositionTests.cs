using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.DependencyInjection;
using MiraiSpace.Presentation.Features.Workspace.Authorization;
using MiraiSpace.Presentation.Features.Workspace.Menu;
using MiraiSpace.Presentation.Menu;
using ReactiveUI;

namespace MiraiSpace.Presentation.Tests.Menu;

public sealed class AppMenuCompositionTests
{
    [Fact]
    public void KeyedRegistrationsComposeRootAndContainerInRegistrationOrder()
    {
        using var provider = CreateProvider();
        using var scope = provider.CreateScope();
        var services = scope.ServiceProvider;
        var menu = services.GetRequiredService<AppMenuViewModel>();
        using var menuActivation = menu.Activator.Activate();

        Assert.Collection(
            menu.Items,
            item => Assert.IsType<OverviewMenuItem>(item),
            item => Assert.IsType<InboxMenuItem>(item),
            item => Assert.IsType<WorkspaceMenuItemContainer>(item),
            item => Assert.IsType<RoleToggleMenuItem>(item));

        var container =
            Assert.IsType<WorkspaceMenuItemContainer>(menu.Items[2]);
        using var containerActivation = container.Activator.Activate();

        Assert.Collection(
            container.Items,
            item => Assert.IsType<WorkspacePagesMenuItem>(item),
            item => Assert.IsType<WorkspaceCalendarMenuItem>(item));
    }

    [Fact]
    public void RoleChangeRevealsRestrictedItemAtItsRegisteredPosition()
    {
        using var provider = CreateProvider();
        using var scope = provider.CreateScope();
        var services = scope.ServiceProvider;
        var menu = services.GetRequiredService<AppMenuViewModel>();
        using var activation = menu.Activator.Activate();

        services.GetRequiredService<CurrentUserContext>().ToggleAdministrator();

        Assert.Collection(
            menu.Items,
            item => Assert.IsType<OverviewMenuItem>(item),
            item => Assert.IsType<InboxMenuItem>(item),
            item => Assert.IsType<WorkspaceMenuItemContainer>(item),
            item => Assert.IsType<AdministrationMenuItem>(item),
            item => Assert.IsType<RoleToggleMenuItem>(item));
    }

    [Fact]
    public void ExtensionCanContributeItemsUsingStableStringKeys()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddPresentation();
        serviceCollection.AddKeyedScoped<IAppMenuItem, ExtensionMenuItem>(AppMenuKeys.Root);
        serviceCollection.AddKeyedScoped<IAppMenuItem, ExtensionMenuItem>(AppMenuKeys.Workspace);

        using var provider = serviceCollection.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var services = scope.ServiceProvider;
        var menu = services.GetRequiredService<AppMenuViewModel>();
        using var menuActivation = menu.Activator.Activate();

        Assert.IsType<ExtensionMenuItem>(menu.Items[^1]);

        var container =
            Assert.IsType<WorkspaceMenuItemContainer>(menu.Items[2]);
        using var containerActivation = container.Activator.Activate();

        Assert.IsType<ExtensionMenuItem>(container.Items[^1]);
    }

    private static ServiceProvider CreateProvider() =>
        new ServiceCollection()
            .AddPresentation()
            .BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

    private sealed class ExtensionMenuItem : IAppMenuItem
    {
        public ICommand ExecuteCommand { get; } =
            ReactiveCommand.Create(() => { });
    }
}
