using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Lifecycle;

namespace MiraiSpace.Presentation.Panels;

public interface IPanelService
{
    IReadOnlyList<NavigationPanelViewModel> Panels { get; }

    Task<Guid> OpenAsync(string address, CancellationToken cancellationToken = default);

    Task<Guid> OpenAsync<TPage, TParameters>(TParameters parameters, CancellationToken cancellationToken = default)
        where TPage : ReactivePage, IInitializable<TParameters>;

    Task CloseAsync(Guid id);

    Task NavigateAsync(Guid id, string address, CancellationToken cancellationToken = default);

    Task GoBackAsync(Guid id, CancellationToken cancellationToken = default);
}
