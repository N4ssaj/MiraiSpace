using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using MessagePipe;
using MiraiSpace.Application.Documents;
using MiraiSpace.Presentation.Foundation;
using MiraiSpace.Presentation.Lifecycle;
using MiraiSpace.Presentation.Panels;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Documents;

public sealed partial class DocumentsPageViewModel : ReactivePage, IInitializable, IDisposable
{
    private readonly IDocumentRepository _documents;
    private readonly IPanelService _panels;
    private readonly ISubscriber<DocumentChanged> _changes;
    private readonly SourceList<Document> _source = new();
    private readonly ReadOnlyObservableCollection<Document> _items;
    private readonly IDisposable _binding;

    public override string Title => "Documents";
    public ReadOnlyObservableCollection<Document> Items => _items;
    public bool HasNoDocuments => Items.Count == 0;

    [Reactive]
    public partial string Search { get; set; } = string.Empty;

    public DocumentsPageViewModel(
        IDocumentRepository documents,
        IPanelService panels,
        ISubscriber<DocumentChanged> changes)
    {
        _documents = documents;
        _panels = panels;
        _changes = changes;
        _binding = _source.Connect()
            .Filter(
                this.WhenAnyValue(model => model.Search),
                (text, document) => document.Name.Contains(text.Trim(), StringComparison.OrdinalIgnoreCase))
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Bind(out _items, new BindingOptions(BindingOptions.DefaultResetThreshold, UseReplaceForUpdates: true))
            .Subscribe(_ => this.RaisePropertyChanged(nameof(HasNoDocuments)));
    }

    public ValueTask InitializeAsync(CancellationToken cancellationToken = default) => ReloadAsync(cancellationToken);

    protected override void OnActivated(CompositeDisposable disposables)
    {
        ((ICommand)RefreshCommand).Execute(null);
        _changes.Subscribe(_ => ((ICommand)RefreshCommand).Execute(null)).DisposeWith(disposables);
    }

    [ReactiveCommand]
    private async Task Open(Document document, CancellationToken cancellationToken)
    {
        await _panels.OpenAsync<DocumentPageViewModel, DocumentOpenParameters>(
            new DocumentOpenParameters
            {
                DocumentId = document.Id,
                Workspace = new WorkspaceReference { Name = "personal", Branch = "draft" },
                View = new DocumentViewOptions { Section = "notes", CursorLine = 3 },
                Tags = ["ideas", "review"]
            },
            cancellationToken);
    }

    [ReactiveCommand]
    private Task Refresh(CancellationToken token) => ReloadAsync(token).AsTask();

    private async ValueTask ReloadAsync(CancellationToken token)
    {
        var documents = await _documents.ListAsync(token);
        _source.Edit(items =>
        {
            items.Clear();
            items.AddRange(documents);
        });
    }

    public void Dispose()
    {
        _binding.Dispose();
        _source.Dispose();
    }
}
