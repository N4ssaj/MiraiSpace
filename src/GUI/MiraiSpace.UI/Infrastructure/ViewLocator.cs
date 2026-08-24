using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.ViewModels;
using ReactiveUI;

namespace MiraiSpace.UI.Infrastructure;

public sealed class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
        {
            return null;
        }

        Type viewType = typeof(IViewFor<>).MakeGenericType(data.GetType());
        return (Control)App.Services.GetRequiredService(viewType);
    }

    public bool Match(object? data) => data is ViewModelBase;
}
