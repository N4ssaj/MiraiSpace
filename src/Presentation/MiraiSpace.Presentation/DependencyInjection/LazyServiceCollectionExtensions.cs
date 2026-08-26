using Microsoft.Extensions.DependencyInjection;

namespace MiraiSpace.Presentation.DependencyInjection;

public static class LazyServiceCollectionExtensions
{
    public static IServiceCollection AddLazy<TService>(this IServiceCollection services)
        where TService : notnull
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddTransient(provider => new Lazy<TService>(
            provider.GetRequiredService<TService>));
        return services;
    }
}
