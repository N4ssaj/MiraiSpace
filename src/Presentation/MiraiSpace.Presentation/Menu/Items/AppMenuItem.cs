using System.Reactive;
using System.Reactive.Disposables;
using System.Windows.Input;
using MiraiSpace.Presentation.Abstractions.Menu;
using MiraiSpace.Presentation.ViewModels;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu.Items;

public abstract class AppMenuItem : ViewModelBase, IAppMenuItem, IOrderedAppMenuItem
{
    protected AppMenuItem(int order = 0)
    {
        Order = order;
        Command = ReactiveCommand.CreateFromTask<Unit, Unit>(
            async (_, cancellationToken) =>
            {
                await ExecuteAsync(cancellationToken);
                return Unit.Default;
            });
    }

    public int Order { get; }

    public ReactiveCommand<Unit, Unit> Command { get; }

    ICommand IAppMenuItem.Command => Command;

    protected abstract ValueTask ExecuteAsync(CancellationToken cancellationToken);

    protected override void OnActivated(CompositeDisposable disposables)
    {
        disposables.Add(Command.ThrownExceptions.Subscribe(OnExecutionError));
    }

    protected virtual void OnExecutionError(Exception exception)
    {
    }

    public override void Dispose()
    {
        Command.Dispose();
        base.Dispose();
    }
}

public abstract class StandardAppMenuItem(int order) : AppMenuItem(order)
{
    public abstract string DisplayTitle { get; }
    public virtual string Caption => string.Empty;
    public virtual string Glyph => "•";
    public virtual string Accent => "#7165E8";
    public virtual string Badge => string.Empty;
    public bool HasBadge => !string.IsNullOrWhiteSpace(Badge);
}
