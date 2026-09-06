using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Standard;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Workspace.Menu;

public sealed partial class WorkspaceMenuItemContainer
    : StandardAppMenuItem, IAppMenuItemContainer
{
    private readonly ReadOnlyObservableCollection<IAppMenuItem> _items;
    private readonly IObservable<IChangeSet<IAppMenuItem>> _itemsPipeline;
    private readonly SourceList<IAppMenuItem> _itemSource = new();
    private readonly IReadOnlyList<IAppMenuAccessPolicy> _policies;

    public override string Title => "Workspace";

    public override string Caption => "Team space";

    public override string Glyph => "◇";

    public override string Accent => "#34A58B";

    [Reactive]
    public partial bool IsExpanded { get; private set; }

    public ReadOnlyObservableCollection<IAppMenuItem> Items => _items;

    IReadOnlyList<IAppMenuItem> IAppMenuItemContainer.Items => Items;

    ICommand IAppMenuItem.ExecuteCommand => ExecuteCommand;

    public WorkspaceMenuItemContainer(
        [FromKeyedServices(AppMenuKeys.Workspace)]
        IEnumerable<IAppMenuItem> items,
        IEnumerable<IAppMenuAccessPolicy> policies)
    {
        _policies = [.. policies];
        IsExpanded = true;
        var itemsPipeline = _itemSource
            .Connect()
            .Filter(
                ObserveAccessInvalidations(),
                (_, item) => CanAccess(item),
                ListFilterPolicy.ClearAndReplace);

        _itemsPipeline = ResetBeforeRebinding(itemsPipeline)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Bind(
                out _items,
                new BindingOptions(
                    BindingOptions.DefaultResetThreshold,
                    UseReplaceForUpdates: true));
        _itemSource.AddRange(items);
    }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        _itemsPipeline
            .Subscribe()
            .DisposeWith(disposables);
    }

    [ReactiveCommand]
    private void Execute() => IsExpanded = !IsExpanded;

    private IObservable<Unit> ObserveAccessInvalidations() =>
        _policies
            .Select(policy => policy.Invalidated)
            .Merge()
            .StartWith(Unit.Default);

    private bool CanAccess(IAppMenuItem item) =>
        _policies.All(policy => policy.CanAccess(item));

    private IObservable<IChangeSet<IAppMenuItem>> ResetBeforeRebinding(
        IObservable<IChangeSet<IAppMenuItem>> source) =>
        Observable.Defer(() =>
        {
            if (_items.Count == 0)
            {
                return source;
            }

            var reset = new ChangeSet<IAppMenuItem>(
                [new Change<IAppMenuItem>(ListChangeReason.Clear, _items.ToArray())]);
            return Observable.Return(reset).Concat(source);
        });
}
