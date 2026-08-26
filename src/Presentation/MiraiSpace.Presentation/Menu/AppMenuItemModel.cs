using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.ViewModels;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu;

public sealed class AppMenuItemModel : ModelBase
{
    private AppMenuItemDescriptor _descriptor;
    private bool _isSelected;

    internal AppMenuItemModel(IAppMenuContribution contribution, int depth)
    {
        Contribution = contribution;
        _descriptor = contribution.Descriptor.Validate();
        Depth = depth;
    }

    internal IAppMenuContribution Contribution { get; }

    public string Id => _descriptor.Id;
    public string Title => _descriptor.Title;
    public string Caption => _descriptor.Caption;
    public string Glyph => _descriptor.Glyph;
    public string Accent => _descriptor.Accent;
    public string Badge => _descriptor.Badge;
    public bool HasBadge => !string.IsNullOrWhiteSpace(Badge);
    public int Depth { get; }
    public double Indent => Depth * 18d;

    public bool IsSelected
    {
        get => _isSelected;
        internal set => this.RaiseAndSetIfChanged(ref _isSelected, value);
    }
}
