using System.Reactive;
using System.Reactive.Disposables;
using MiraiSpace.Presentation.Menu;
using MiraiSpace.Presentation.Menu.Demo;
using ReactiveUI;

namespace MiraiSpace.Presentation.ViewModels;

public sealed class MainViewModel : ViewModelBase
{
    private string? _lastExecutionError;

    public MainViewModel(IAppMenuViewModel menu, AppNavigationState navigation)
    {
        Menu = menu;
        Navigation = navigation;
        ExecuteMenuItemCommand = ReactiveCommand.CreateFromTask<AppMenuItemModel>(
            async item => await Menu.ExecuteAsync(item));
    }

    public IAppMenuViewModel Menu { get; }
    public IReadOnlyList<AppMenuItemModel> Items => Menu.Items;
    public AppNavigationState Navigation { get; }
    public ReactiveCommand<AppMenuItemModel, Unit> ExecuteMenuItemCommand { get; }

    public string? LastExecutionError
    {
        get => _lastExecutionError;
        private set => this.RaiseAndSetIfChanged(ref _lastExecutionError, value);
    }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        disposables.Add(ExecuteMenuItemCommand.ThrownExceptions
            .Subscribe(exception => LastExecutionError = exception.Message));

        if (Menu is IActivatableViewModel activatableMenu)
        {
            disposables.Add(activatableMenu.Activator.Activate());
        }
    }
}
