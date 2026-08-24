using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.Menu.Demo;
using MiraiSpace.UI.Views.Menu;
using ReactiveUI;

namespace MiraiSpace.UI.DependencyInjection;

public static class ViewRegistration
{
    public static void Register(IServiceCollection services)
    {
        services.AddTransient<IViewFor<MenuItemViewModel>, MenuItemView>();
    }
}
