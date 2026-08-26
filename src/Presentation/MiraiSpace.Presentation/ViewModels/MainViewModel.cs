using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu;
using MiraiSpace.Presentation.Menu.Demo;
using System.Reactive;
using ReactiveUI;

namespace MiraiSpace.Presentation.ViewModels;

public sealed class MainViewModel : ViewModelBase
{
    private string? _lastExecutionError;

    public MainViewModel(IAppMenuViewModel menu, AppNavigationState navigation)
    {
        Menu = menu;
        Navigation = navigation;
        ExecuteMenuItemCommand = ReactiveCommand.CreateFromTask<IAppMenuItem>(
            async item => await Menu.ExecuteAsync(item));
        Own(ExecuteMenuItemCommand);
        Own(ExecuteMenuItemCommand.ThrownExceptions
            .Subscribe(exception => LastExecutionError = exception.Message));
    }

    public IAppMenuViewModel Menu { get; }

    public IReadOnlyList<IAppMenuItem> Items => Menu.Items;

    public AppNavigationState Navigation { get; }

    public ReactiveCommand<IAppMenuItem, Unit> ExecuteMenuItemCommand { get; }

    public string? LastExecutionError
    {
        get => _lastExecutionError;
        private set => this.RaiseAndSetIfChanged(ref _lastExecutionError, value);
    }
}
