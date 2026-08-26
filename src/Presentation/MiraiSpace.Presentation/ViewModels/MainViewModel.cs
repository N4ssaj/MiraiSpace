using System.Reactive.Disposables;
using System.Reactive;
using MiraiSpace.Presentation.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Demo;
using ReactiveUI;

namespace MiraiSpace.Presentation.ViewModels;

public sealed class MainViewModel : ViewModelBase
{
    private bool _isMenuOpen = true;

    public MainViewModel(IAppMenu menu, AppNavigationState navigation)
    {
        Menu = menu;
        Navigation = navigation;
        ToggleMenuCommand = ReactiveCommand.Create(() =>
        {
            IsMenuOpen = !IsMenuOpen;
        });
    }

    public IAppMenu Menu { get; }

    public IReadOnlyList<IAppMenuItem> Items => Menu.Items;

    public AppNavigationState Navigation { get; }

    public bool IsMenuOpen
    {
        get => _isMenuOpen;
        set => this.RaiseAndSetIfChanged(ref _isMenuOpen, value);
    }

    public ReactiveCommand<Unit, Unit> ToggleMenuCommand { get; }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        if (Menu is IActivatableViewModel activatable)
        {
            disposables.Add(activatable.Activator.Activate());
        }
    }

    public override void Dispose()
    {
        ToggleMenuCommand.Dispose();
        base.Dispose();
    }
}
