using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Presentation.Menu.Demo;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace MiraiSpace.UI.Views;

public static class MenuItemViewRegistration
{
    public static IServiceCollection AddMenuItemViews(this IServiceCollection services)
    {
        services.AddSingleton<ViewLocator>();
        services.AddSingleton<AppMenuItemChildrenSelector>();
        services.AddTransient<MainView>();
        services.AddTransient<MainWindow>();

        services.AddTransient<IViewFor<DashboardMenuItem>, DashboardMenuItemView>();
        services.AddTransient<IViewFor<InboxMenuItem>, InboxMenuItemView>();
        services.AddTransient<IViewFor<WorkspaceMenuItemContainer>, WorkspaceMenuItemView>();
        services.AddTransient<IViewFor<AdministrationMenuItem>, AdministrationMenuItemView>();
        services.AddTransient<IViewFor<RoleToggleMenuItem>, RoleToggleMenuItemView>();
        services.AddTransient<IViewFor<WorkspacePageMenuItem>, WorkspacePageMenuItemView>();
        services.AddTransient<IViewFor<WorkspaceCalendarMenuItem>, WorkspaceCalendarMenuItemView>();
        services.AddTransient<IViewFor<DelegateMenuItem>, DelegateMenuItemView>();
        return services;
    }
}

public sealed class DashboardMenuItemView : MenuItemView<DashboardMenuItem>
{
    public DashboardMenuItemView() : base(MenuItemVisualKind.Standard) { }
}

public sealed class InboxMenuItemView : MenuItemView<InboxMenuItem>
{
    public InboxMenuItemView() : base(MenuItemVisualKind.Badge) { }
}

public sealed class WorkspaceMenuItemView : MenuItemView<WorkspaceMenuItemContainer>
{
    public WorkspaceMenuItemView() : base(MenuItemVisualKind.Group) { }
}

public sealed class AdministrationMenuItemView : MenuItemView<AdministrationMenuItem>
{
    public AdministrationMenuItemView() : base(MenuItemVisualKind.Restricted) { }
}

public sealed class RoleToggleMenuItemView : MenuItemView<RoleToggleMenuItem>
{
    public RoleToggleMenuItemView() : base(MenuItemVisualKind.Action) { }
}

public sealed class WorkspacePageMenuItemView : MenuItemView<WorkspacePageMenuItem>
{
    public WorkspacePageMenuItemView() : base(MenuItemVisualKind.WorkspaceLeaf) { }
}

public sealed class WorkspaceCalendarMenuItemView : MenuItemView<WorkspaceCalendarMenuItem>
{
    public WorkspaceCalendarMenuItemView() : base(MenuItemVisualKind.WorkspaceLeaf) { }
}

public sealed class DelegateMenuItemView : MenuItemView<DelegateMenuItem>
{
    public DelegateMenuItemView() : base(MenuItemVisualKind.Delegate) { }
}

public abstract class MenuItemView<TViewModel> : ReactiveUserControl<TViewModel>
    where TViewModel : MenuItemViewModel
{
    protected MenuItemView(MenuItemVisualKind kind)
    {
        HorizontalContentAlignment = HorizontalAlignment.Stretch;
        Content = MenuItemVisual.Create(kind);
    }
}

public enum MenuItemVisualKind
{
    Standard,
    Badge,
    Group,
    Restricted,
    Action,
    WorkspaceLeaf,
    Delegate
}

internal static class MenuItemVisual
{
    public static Control Create(MenuItemVisualKind kind)
    {
        var grid = new Grid { ColumnDefinitions = new ColumnDefinitions("42,*,Auto") };
        grid.Children.Add(CreateGlyph(kind));

        var labels = new StackPanel { Margin = new Avalonia.Thickness(11, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center };
        labels.Children.Add(new TextBlock
        {
            [!TextBlock.TextProperty] = new Binding(nameof(MenuItemViewModel.DisplayTitle)),
            FontWeight = FontWeight.SemiBold,
            Foreground = Brush.Parse("#343640")
        });
        labels.Children.Add(new TextBlock
        {
            [!TextBlock.TextProperty] = new Binding(nameof(MenuItemViewModel.Caption)),
            FontSize = 9,
            Foreground = Brush.Parse(kind == MenuItemVisualKind.Group ? "#34A58B" : "#9295A1")
        });
        Grid.SetColumn(labels, 1);
        grid.Children.Add(labels);

        Control? trailing = CreateTrailing(kind);
        if (trailing is not null)
        {
            Grid.SetColumn(trailing, 2);
            grid.Children.Add(trailing);
        }

        return grid;
    }

    private static Control CreateGlyph(MenuItemVisualKind kind) => new Border
    {
        Width = 38,
        Height = 38,
        CornerRadius = new Avalonia.CornerRadius(kind == MenuItemVisualKind.Delegate ? 19 : 11),
        [!Border.BackgroundProperty] = new Binding(nameof(MenuItemViewModel.Accent)),
        Child = new TextBlock
        {
            [!TextBlock.TextProperty] = new Binding(nameof(MenuItemViewModel.Glyph)),
            FontSize = kind == MenuItemVisualKind.Delegate ? 11 : 17,
            FontWeight = FontWeight.SemiBold,
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        }
    };

    private static Control? CreateTrailing(MenuItemVisualKind kind) => kind switch
    {
        MenuItemVisualKind.Badge => new Border
        {
            CornerRadius = new Avalonia.CornerRadius(10),
            Padding = new Avalonia.Thickness(7, 2),
            VerticalAlignment = VerticalAlignment.Center,
            [!Border.BackgroundProperty] = new Binding(nameof(MenuItemViewModel.Accent)),
            Child = new TextBlock
            {
                [!TextBlock.TextProperty] = new Binding(nameof(MenuItemViewModel.Badge)),
                FontSize = 10,
                FontWeight = FontWeight.Bold,
                Foreground = Brushes.White
            }
        },
        MenuItemVisualKind.Group => CreateText("⌄", "#34A58B"),
        MenuItemVisualKind.Restricted => CreateText("LOCKED", "#A151C2", 8),
        MenuItemVisualKind.Action => CreateText("SWITCH", "#7165E8", 8),
        MenuItemVisualKind.WorkspaceLeaf => CreateText("›", "#9295A1", 16),
        MenuItemVisualKind.Delegate => CreateText("SHARED", "#9295A1", 8),
        _ => null
    };

    private static TextBlock CreateText(string text, string color, double fontSize = 12) => new()
    {
        Text = text,
        FontSize = fontSize,
        FontWeight = FontWeight.Bold,
        Foreground = Brush.Parse(color),
        VerticalAlignment = VerticalAlignment.Center
    };
}
