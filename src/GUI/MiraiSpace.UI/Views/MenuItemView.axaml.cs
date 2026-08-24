using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.Menu.Demo;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace MiraiSpace.UI.Views;

public partial class MenuItemView : ReactiveUserControl<MenuItemViewModel>
{
    public MenuItemView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

public static class MenuItemViewRegistration
{
    public static IServiceCollection AddMenuItemViews(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<MenuItemViewModel>, MenuItemView>();
        return services;
    }
}
