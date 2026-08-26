using MiraiSpace.Presentation.ViewModels;
using ReactiveUI.Avalonia;

namespace MiraiSpace.UI.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
    }
}
