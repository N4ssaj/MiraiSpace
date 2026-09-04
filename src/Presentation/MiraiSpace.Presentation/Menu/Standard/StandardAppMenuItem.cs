using MiraiSpace.Presentation.Foundation;

namespace MiraiSpace.Presentation.Menu.Standard;

public abstract class StandardAppMenuItem : ReactiveComponent
{
    public abstract string Title { get; }

    public virtual string? Caption => null;

    public virtual string Glyph => "•";

    public virtual string Accent => "#7165E8";
}
