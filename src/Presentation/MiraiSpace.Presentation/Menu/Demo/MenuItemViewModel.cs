using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.ViewModels;

namespace MiraiSpace.Presentation.Menu.Demo;

public abstract class MenuItemViewModel(
    AppNavigationState navigation,
    string id,
    int order) : ViewModelBase, IAppMenuItem
{
    protected AppNavigationState Navigation { get; } = navigation;

    public string Id { get; } = id;

    public int Order { get; } = order;

    public virtual string DisplayTitle => Id;

    public virtual string Caption => string.Empty;

    public virtual string Glyph => "•";

    public virtual string Accent => "#7165E8";

    public virtual string Badge => string.Empty;

    public bool HasBadge => !string.IsNullOrWhiteSpace(Badge);

    public abstract ValueTask ExecuteAsync(CancellationToken cancellationToken = default);
}
