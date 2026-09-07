using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Lifecycle;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Documents;

public sealed partial class DocumentNameComponent : ReactiveComponent, IInitializable<string>
{
    [Reactive]
    public partial string Name { get; set; } = string.Empty;

    public int MaximumLength { get; } = 120;

    public ValueTask InitializeAsync(string name, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Name = name;
        return ValueTask.CompletedTask;
    }
}
