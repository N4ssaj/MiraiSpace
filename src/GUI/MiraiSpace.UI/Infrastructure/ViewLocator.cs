using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace MiraiSpace.UI.Infrastructure;

public sealed class ViewLocator : IDataTemplate
{
    private readonly IServiceProvider _services;

    public ViewLocator(IServiceProvider services)
    {
        _services = services;
    }

    public Control? Build(object? data)
    {
        if (data is null)
        {
            return null;
        }

        var viewType = typeof(IViewFor<>).MakeGenericType(data.GetType());
        return _services.GetService(viewType) as Control;
    }

    public bool Match(object? data) => true;
}
