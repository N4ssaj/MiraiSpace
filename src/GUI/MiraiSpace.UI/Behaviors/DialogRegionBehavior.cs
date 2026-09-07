using Avalonia.VisualTree;
using System.ComponentModel;
using Avalonia;
using Avalonia.Xaml.Interactivity;
using DialogHostAvalonia;
using MiraiSpace.Presentation.Dialogs;
using MiraiSpace.UI.Dialogs;

namespace MiraiSpace.UI.Behaviors;

public sealed class DialogRegionBehavior : Behavior<DialogHost>
{
    public static readonly StyledProperty<IDialogService?> ServiceProperty =
        AvaloniaProperty.Register<DialogRegionBehavior, IDialogService?>(nameof(Service));
    public static readonly StyledProperty<INotifyPropertyChanged?> OwnerProperty =
        AvaloniaProperty.Register<DialogRegionBehavior, INotifyPropertyChanged?>(nameof(Owner));
    private IDisposable? _registration;

    public IDialogService? Service
    {
        get => GetValue(ServiceProperty);
        set => SetValue(ServiceProperty, value);
    }

    public INotifyPropertyChanged? Owner
    {
        get => GetValue(OwnerProperty);
        set => SetValue(OwnerProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject!.AttachedToVisualTree += OnAttachedToVisualTree;
        AssociatedObject.DetachedFromVisualTree += OnDetachedFromVisualTree;
        Register();
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is { } host)
        {
            host.AttachedToVisualTree -= OnAttachedToVisualTree;
            host.DetachedFromVisualTree -= OnDetachedFromVisualTree;
        }

        Unregister();
        base.OnDetaching();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ServiceProperty || change.Property == OwnerProperty)
        {
            Register();
        }
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs args) => Register();

    private void OnDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs args) => Unregister();

    private void Unregister()
    {
        _registration?.Dispose();
        _registration = null;
    }

    private void Register()
    {
        _registration?.Dispose();
        _registration = null;
        if (AssociatedObject is { } host && host.IsAttachedToVisualTree() && Owner is { } owner && Service is AvaloniaDialogService service)
        {
            _registration = service.Register(owner, host);
        }
    }
}
