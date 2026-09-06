using Microsoft.Extensions.DependencyInjection;

namespace MiraiSpace.Extensibility.Abstractions.Modules;

public static class AppModuleServiceCollectionExtensions
{
    public static IServiceCollection AddModule<TModule>(this IServiceCollection services)
        where TModule : IAppModule, new()
    {
        ArgumentNullException.ThrowIfNull(services);
        new TModule().ConfigureServices(services);
        return services;
    }
}
