using HanumanInstitute.MvvmDialogs;
using MiraiSpace.Application.Documents;
using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Lifecycle;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Documents;

public sealed partial class DeleteDocumentDialog : ReactiveComponent, IModalDialogViewModel, ICloseable, IInitializable<Guid>
{
    private readonly IDocumentRepository _documents;

    [Reactive]
    public partial string Name { get; private set; } = string.Empty;

    public bool? DialogResult { get; private set; }
    public event EventHandler? RequestClose;

    public DeleteDocumentDialog(IDocumentRepository documents)
    {
        _documents = documents;
    }

    public async ValueTask InitializeAsync(Guid documentId, CancellationToken cancellationToken = default)
    {
        var document = await _documents.GetAsync(documentId, cancellationToken)
            ?? throw new InvalidOperationException("The document no longer exists.");
        Name = document.Name;
    }

    [ReactiveCommand]
    private void Confirm()
    {
        DialogResult = true;
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    [ReactiveCommand]
    private void Cancel()
    {
        DialogResult = null;
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
}
