using System.Reactive;
using System.Reactive.Disposables;
using System.Windows.Input;
using MiraiSpace.Presentation.Abstractions.Menu;
using MiraiSpace.Presentation.ViewModels;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Standard;

public abstract class StandardAppMenuItem : ViewModelBase, IAppMenuItem, IOrderedAppMenuItem
{
    private string? _lastError;

    protected StandardAppMenuItem(int order)
    {
        Order = order;
        Command = ReactiveCommand.CreateFromTask(ExecuteAsync);
    }

    public int Order { get; }
    public abstract string Title { get; }
    public virtual string Caption => string.Empty;
    public virtual string Glyph => "•";
    public virtual string Accent => "#7165E8";
    public virtual string Badge => string.Empty;
    public bool HasBadge => !string.IsNullOrWhiteSpace(Badge);
    public string? LastError
    {
        get => _lastError;
        private set => this.RaiseAndSetIfChanged(ref _lastError, value);
    }

    public ReactiveCommand<Unit, Unit> Command { get; }
    ICommand IAppMenuItem.Command => Command;

    protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

    protected override void OnActivated(CompositeDisposable disposables)
    {
        disposables.Add(Command.ThrownExceptions
            .Subscribe(exception => LastError = exception.Message));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) Command.Dispose();
        base.Dispose(disposing);
    }
}
