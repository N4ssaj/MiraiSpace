using Microsoft.Extensions.DependencyInjection;

namespace MiraiSpace.Extensibility.Abstractions.Menu;

public static class AppMenuServiceCollectionExtensions
{
    public static IServiceCollection AddAppMenuItem<TItem>(
        this IServiceCollection services,
        string key)
        where TItem : class, IAppMenuItem
    {
        services.AddKeyedSingleton<IAppMenuItem, TItem>(key);
        return services;
    }
}
