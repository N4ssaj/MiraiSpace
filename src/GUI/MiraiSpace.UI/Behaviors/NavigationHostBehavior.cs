using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using MiraiSpace.Presentation.Navigation;
using MiraiSpace.UI.Navigation;

namespace MiraiSpace.UI.Behaviors;

public sealed class NavigationHostBehavior : Behavior<NavigationPage>
{
    public static readonly StyledProperty<INavigationHost?> HostProperty =
        AvaloniaProperty.Register<NavigationHostBehavior, INavigationHost?>(nameof(Host));

    public INavigationHost? Host
    {
        get => GetValue(HostProperty);
        set => SetValue(HostProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AttachHost();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == HostProperty)
        {
            AttachHost();
        }
    }

    private void AttachHost()
    {
        if (AssociatedObject is { } control && Host is AvaloniaNavigationHost host)
        {
            host.Attach(control);
        }
    }
}
