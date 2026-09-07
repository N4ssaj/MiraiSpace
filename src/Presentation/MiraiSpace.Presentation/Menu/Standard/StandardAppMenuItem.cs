using MiraiSpace.Presentation.Foundation;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Menu.Standard;

public abstract partial class StandardAppMenuItem : ReactiveComponent
{
    [Reactive] public partial bool IsSelected { get; protected set; }

    public abstract string Title { get; }

    public virtual string? Caption => null;

    public virtual string Glyph => "•";

    public virtual string Accent => "#7165E8";
}
