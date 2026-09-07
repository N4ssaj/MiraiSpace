using System.Reactive.Linq;
using HanumanInstitute.MvvmDialogs;
using MiraiSpace.Application.Documents;
using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Lifecycle;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Documents;

public sealed partial class RenameDocumentDialog : ReactiveComponent, IModalDialogViewModel, ICloseable, IInitializable<Guid>
{
    private readonly IDocumentRepository _documents;
    private readonly Initialization<Guid> _initialization;
    private string _name = string.Empty;

    public DocumentNameComponent Form { get; }
    public bool? DialogResult { get; private set; }
    public event EventHandler? RequestClose;

    private IObservable<bool> CanSave => Form.WhenAnyValue(
        model => model.Name,
        name => !string.IsNullOrWhiteSpace(name) && name.Trim().Length <= Form.MaximumLength);

    public RenameDocumentDialog(IDocumentRepository documents, DocumentNameComponent form)
    {
        _documents = documents;
        Form = form;
        _initialization = new Initialization<Guid>(LoadAsync).Then(form, _ => _name);
    }

    public ValueTask InitializeAsync(Guid documentId, CancellationToken cancellationToken = default) =>
        _initialization.RunAsync(documentId, cancellationToken);

    private async ValueTask LoadAsync(Guid documentId, CancellationToken token)
    {
        var document = await _documents.GetAsync(documentId, token)
            ?? throw new InvalidOperationException("The document no longer exists.");
        _name = document.Name;
    }

    [ReactiveCommand(CanExecute = nameof(CanSave))]
    private void Save()
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
