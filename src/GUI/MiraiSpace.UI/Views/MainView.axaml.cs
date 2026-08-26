using System.Reactive.Linq;
using MiraiSpace.Presentation.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace MiraiSpace.UI.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            if (ViewModel is null) return;
            disposables.Add(ViewModel.WhenAnyValue(model => model.IsMenuOpen)
                .Where(isOpen => !isOpen)
                .Subscribe(_ => MenuTree.CollapseAllNodes()));
        });
    }
}
