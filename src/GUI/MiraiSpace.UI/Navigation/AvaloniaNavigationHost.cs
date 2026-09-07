using Avalonia.Controls;
using Avalonia.Threading;
using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Navigation;
using ReactiveUI;
using ViewLocator = MiraiSpace.UI.Infrastructure.ViewLocator;

namespace MiraiSpace.UI.Navigation;

/// <summary>Translates model stack operations to the panel's native NavigationPage.</summary>
public sealed class AvaloniaNavigationHost : INavigationHost
{
    private readonly ViewLocator _views;
    private readonly TaskCompletionSource<NavigationPage> _ready = new();
    private NavigationPage? _control;

    public ReactivePage? Current => _control?.NavigationStack.LastOrDefault()?.DataContext as ReactivePage;
    public int Count => _control?.NavigationStack.Count ?? 0;

    public AvaloniaNavigationHost(ViewLocator views)
    {
        _views = views;
    }

    internal void Attach(NavigationPage control)
    {
        if (_control is not null && !ReferenceEquals(_control, control))
        {
            throw new InvalidOperationException("Each navigation panel requires its own scope.");
        }

        _control = control;
        control.IsBackButtonVisible = false;
        control.IsGestureEnabled = false;
        _ready.TrySetResult(control);
    }

    public Task PushAsync(ReactivePage model, CancellationToken cancellationToken) =>
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var control = await _ready.Task.WaitAsync(cancellationToken);
            var view = _views.Build(model)
                ?? throw new InvalidOperationException($"No view is registered for {model.GetType().Name}.");
            ((IViewFor)view).ViewModel = model;
            var page = new ContentPage { Content = view, DataContext = model, Header = model.Title };
            page.DataTemplates.Add(_views);
            NavigationPage.SetHasNavigationBar(page, false);
            await control.PushAsync(page);
            if (!control.NavigationStack.Contains(page))
            {
                throw new InvalidOperationException("The native page rejected navigation.");
            }
        });

    public Task PopAsync(CancellationToken cancellationToken) =>
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var control = await _ready.Task.WaitAsync(cancellationToken);
            if (control.NavigationStack.Count > 1)
            {
                await control.PopAsync();
            }
        });
}
