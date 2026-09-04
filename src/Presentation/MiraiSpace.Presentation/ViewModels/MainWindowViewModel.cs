using MiraiSpace.Presentation.Foundation;

namespace MiraiSpace.Presentation.ViewModels;

public sealed class MainWindowViewModel : ReactivePage
{
    public MainViewModel Main { get; }

    public MainWindowViewModel(MainViewModel main)
    {
        Main = main;
    }
}
