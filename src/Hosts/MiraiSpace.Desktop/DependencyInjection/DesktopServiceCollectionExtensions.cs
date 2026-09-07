using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.DependencyInjection;
using MiraiSpace.UI.DependencyInjection;

namespace MiraiSpace.Desktop.DependencyInjection;

public static class DesktopServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDesktopApplication()
        {
            return services.AddPresentation().AddUi();
        }
    }
}
