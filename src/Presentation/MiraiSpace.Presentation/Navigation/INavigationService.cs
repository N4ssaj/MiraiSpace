using System.ComponentModel;
using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Lifecycle;

namespace MiraiSpace.Presentation.Navigation;

public interface INavigationService : INotifyPropertyChanged
{
    ReactivePage? Current { get; }
    NavigationAddress? Address { get; }
    bool IsNavigating { get; }

    Task NavigateAsync(string address, CancellationToken cancellationToken = default);

    Task NavigateAsync<TPage>(CancellationToken cancellationToken = default)
        where TPage : ReactivePage;

    Task NavigateAsync<TPage, TParameters>(TParameters parameters, CancellationToken cancellationToken = default)
        where TPage : ReactivePage, IInitializable<TParameters>;

    Task GoBackAsync(CancellationToken cancellationToken = default);
}
