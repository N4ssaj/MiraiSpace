using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.Menu.Demo;
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

        Type viewModelType = data is MenuItemViewModel
            ? typeof(MenuItemViewModel)
            : data.GetType();
        Type viewType = typeof(IViewFor<>).MakeGenericType(viewModelType);
        return (Control)App.Services.GetRequiredService(viewType);
    }

    public bool Match(object? data) => data is ViewModelBase;
}
