using Avalonia;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using Eremex.AvaloniaUI.Controls.Docking;
using MiraiSpace.Presentation.Panels;

namespace MiraiSpace.UI.Behaviors;

/// <summary>Places generated navigation panes in floating windows and disables docking.</summary>
public sealed class FloatingPanelsBehavior : Behavior<DockManager>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject!.RegisterDockItem += OnRegisterDockItem;
        AssociatedObject.DockOperationStarting += OnDockOperationStarting;
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is { } manager)
        {
            manager.RegisterDockItem -= OnRegisterDockItem;
            manager.DockOperationStarting -= OnDockOperationStarting;
        }

        base.OnDetaching();
    }

    private void OnRegisterDockItem(object? sender, DockItemEventArgs args)
    {
        if (args.Item is not DocumentPane pane)
        {
            return;
        }

        Dispatcher.UIThread.Post(() =>
        {
            if (AssociatedObject is not { } manager || pane.DockParent is null || pane.DataContext is not NavigationPanelViewModel)
            {
                return;
            }

            manager.Float(pane);
            if (pane.FloatGroup is { } window)
            {
                window.FloatWidth = 1080;
                window.FloatHeight = 740;
                window.FloatLocation = manager.PointToScreen(new Point(48, 48));
            }
        });
    }

    private static void OnDockOperationStarting(object? sender, DockOperationStartingEventArgs args)
    {
        if (args.DockOperation == DockOperation.Dock)
        {
            args.Cancel = true;
        }
    }
}
