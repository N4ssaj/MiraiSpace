using Avalonia.Controls;

namespace MiraiSpace.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainView mainView)
    {
        InitializeComponent();
        Content = mainView;
    }
}
