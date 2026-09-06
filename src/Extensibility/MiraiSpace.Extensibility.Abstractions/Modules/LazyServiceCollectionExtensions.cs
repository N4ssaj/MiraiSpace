using Microsoft.Extensions.DependencyInjection;

namespace MiraiSpace.Extensibility.Abstractions.Modules;

public static class LazyServiceCollectionExtensions
{
    public static IServiceCollection AddLazyResolution(this IServiceCollection services)
    {
        services.AddTransient(typeof(Lazy<>), typeof(DependencyLazy<>));
        return services;
    }

    private sealed class DependencyLazy<T>(IServiceProvider services)
        : Lazy<T>(services.GetRequiredService<T>) where T : notnull;
}
