using MiraiSpace.Presentation.ViewModels;
using ReactiveUI.Avalonia;

namespace MiraiSpace.UI.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(MainWindowViewModel viewModel)
        : this()
    {
        ViewModel = viewModel;
    }
}
