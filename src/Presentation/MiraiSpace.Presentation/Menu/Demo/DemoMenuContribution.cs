using System.Reactive;
using System.Reactive.Linq;
using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu.Demo;

public abstract class DemoMenuContribution(
    AppNavigationState navigation,
    AppMenuItemDescriptor descriptor) : IAppMenuContribution
{
    protected AppNavigationState Navigation { get; } = navigation;

    public virtual AppMenuItemDescriptor Descriptor { get; } = descriptor.Validate();

    public virtual IObservable<Unit> Changed => Observable.Never<Unit>();

    public abstract ValueTask ExecuteAsync(CancellationToken cancellationToken = default);

    protected ValueTask NavigateAsync(
        string eyebrow,
        string title,
        string description,
        string? accent = null)
    {
        Navigation.Navigate(
            Descriptor.Id,
            eyebrow,
            title,
            description,
            accent ?? Descriptor.Accent);
        return ValueTask.CompletedTask;
    }
}
