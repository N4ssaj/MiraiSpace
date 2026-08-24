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

        Type viewModelType = data.GetType();
        while (viewModelType != typeof(ViewModelBase))
        {
            Type viewType = typeof(IViewFor<>).MakeGenericType(viewModelType);
            object? view = App.Services.GetService(viewType);
            if (view is not null)
            {
                return (Control)view;
            }

            viewModelType = viewModelType.BaseType!;
        }

        return (Control)App.Services.GetRequiredService<IViewFor<ViewModelBase>>();
    }

    public bool Match(object? data) => data is ViewModelBase;
}
