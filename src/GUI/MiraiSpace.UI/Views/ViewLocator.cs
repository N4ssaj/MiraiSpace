using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.Menu.Demo;
using ReactiveUI;

namespace MiraiSpace.UI.Views;

public sealed class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
        {
            return null;
        }

        object resolvedView = App.Services.GetRequiredService<IViewFor<MenuItemViewModel>>();
        if (resolvedView is not IViewFor view || resolvedView is not Control control)
        {
            throw new InvalidOperationException($"The view for {data.GetType().Name} is not an Avalonia ReactiveUI control.");
        }

        view.ViewModel = data;
        return control;
    }

    public bool Match(object? data) => data is MenuItemViewModel;
}
