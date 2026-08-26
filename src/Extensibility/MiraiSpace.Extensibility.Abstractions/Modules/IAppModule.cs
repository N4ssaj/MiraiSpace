using Microsoft.Extensions.DependencyInjection;

namespace MiraiSpace.Extensibility.Abstractions.Modules;

/// <summary>
/// Shared entry point used by built-in modules and trusted external plugins.
/// </summary>
public interface IAppModule
{
    void ConfigureServices(IServiceCollection services);
}
