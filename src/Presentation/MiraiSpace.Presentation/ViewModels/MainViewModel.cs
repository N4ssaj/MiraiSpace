using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu;
using MiraiSpace.Presentation.Menu.Demo;
using ReactiveUI;
using System.Reactive;

namespace MiraiSpace.Presentation.ViewModels;

public sealed class MainViewModel : ViewModelBase, IDisposable
{
    private readonly IDisposable _executionErrorSubscription;
    private string? _lastExecutionError;

    public MainViewModel(IAppMenuViewModel menu, AppNavigationState navigation)
    {
        Menu = menu;
        Navigation = navigation;
        ExecuteMenuItemCommand = ReactiveCommand.CreateFromTask<IAppMenuItem>(
            async item => await Menu.ExecuteAsync(item));
        _executionErrorSubscription = ExecuteMenuItemCommand.ThrownExceptions
            .Subscribe(exception => LastExecutionError = exception.Message);
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

    public void Dispose()
    {
        _executionErrorSubscription.Dispose();
        ExecuteMenuItemCommand.Dispose();
    }
}
