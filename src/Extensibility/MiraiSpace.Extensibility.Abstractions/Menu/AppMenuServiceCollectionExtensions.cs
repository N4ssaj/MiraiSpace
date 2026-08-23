using Microsoft.Extensions.DependencyInjection;

namespace MiraiSpace.Extensibility.Abstractions.Menu;

public static class AppMenuServiceCollectionExtensions
{
    public static IServiceCollection AddAppMenuItem<TItem>(
        this IServiceCollection services,
        AppMenuKey key)
        where TItem : class, IAppMenuItem
    {
        services.AddKeyedSingleton<IAppMenuItem, TItem>(key.Value);
        return services;
    }
}
