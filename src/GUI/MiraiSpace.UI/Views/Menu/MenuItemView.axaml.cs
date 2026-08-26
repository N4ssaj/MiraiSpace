using Avalonia;
using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace MiraiSpace.UI.Views.Menu;

public partial class MenuItemView : ReactiveUserControl<StandardAppMenuItem>
{
    public MenuItemView() => InitializeComponent();

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == MenuDisplay.IsCompactProperty)
        {
            Details.IsVisible = !change.GetNewValue<bool>();
        }
    }
}

public sealed class MenuItemView<TViewModel> : MenuItemView, IViewFor<TViewModel>
    where TViewModel : StandardAppMenuItem
{
    TViewModel? IViewFor<TViewModel>.ViewModel
    {
        get => DataContext as TViewModel;
        set => DataContext = value;
    }
}
