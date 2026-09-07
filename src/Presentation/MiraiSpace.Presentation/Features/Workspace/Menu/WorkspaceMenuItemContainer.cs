using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Standard;
using MiraiSpace.Presentation.Foundation;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace MiraiSpace.Presentation.Features.Workspace.Menu;

public sealed partial class WorkspaceMenuItemContainer
    : StandardAppMenuItem, IAppMenuItemContainer, IDisposable
{
    private readonly ReadOnlyObservableCollection<IAppMenuItem> _items;
    private readonly IDisposable _binding;
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
        _binding = _itemSource
            .Connect()
            .Filter(
                ObserveAccessInvalidations(),
                (_, item) => CanAccess(item),
                ListFilterPolicy.ClearAndReplace)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Bind(
                out _items,
                new BindingOptions(
                    BindingOptions.DefaultResetThreshold,
                    UseReplaceForUpdates: true))
            .Subscribe();
        _itemSource.AddRange(items);
    }

    [ReactiveCommand]
    private void Execute() => IsExpanded = !IsExpanded;

    public void Dispose()
    {
        _itemSource.Dispose();
        _binding.Dispose();
    }

    private IObservable<Unit> ObserveAccessInvalidations() =>
        _policies
            .Select(policy => policy.Invalidated)
            .Merge()
            .StartWith(Unit.Default);

    private bool CanAccess(IAppMenuItem item) =>
        _policies.All(policy => policy.CanAccess(item));

}
