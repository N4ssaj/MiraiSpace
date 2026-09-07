using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Sample.Plugin.DependencyInjection;
using ReactiveMarbles.Extensions.Hosting.Plugins;

namespace MiraiSpace.Sample.Plugin;

public sealed class Plugin : IPlugin
{
    public void ConfigureHost(object hostBuilderContext, IServiceCollection serviceCollection)
    {
        serviceCollection.AddSamplePlugin();
    }
}
