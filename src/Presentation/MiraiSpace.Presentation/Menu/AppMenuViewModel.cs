using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using MiraiSpace.Extensibility.Abstractions.Menu;
using MiraiSpace.Presentation.Menu.Demo;
using MiraiSpace.Presentation.ViewModels;
using ReactiveUI;

namespace MiraiSpace.Presentation.Menu;

public sealed class AppMenuViewModel : ComponentViewModelBase, IAppMenuViewModel
{
    private readonly IAppMenuContribution[] _contributions;
    private readonly IAppMenuAccessEvaluator _accessEvaluator;
    private readonly IAppMenuContributionExecutor _executor;
    private readonly AppNavigationState _navigation;
    private readonly ObservableCollection<AppMenuItemModel> _items = [];
    private readonly ReadOnlyObservableCollection<AppMenuItemModel> _readOnlyItems;
    private AppMenuItemModel? _selectedItem;

    public AppMenuViewModel(
        IEnumerable<IAppMenuContribution> contributions,
        IAppMenuAccessEvaluator accessEvaluator,
        IAppMenuContributionExecutor executor,
        AppNavigationState navigation)
        : base("application-menu")
    {
        _contributions = contributions.ToArray();
        _accessEvaluator = accessEvaluator;
        _executor = executor;
        _navigation = navigation;
        _readOnlyItems = new ReadOnlyObservableCollection<AppMenuItemModel>(_items);
        Recompose();
    }

    public IReadOnlyList<AppMenuItemModel> Items => _readOnlyItems;

    public AppMenuItemModel? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public async ValueTask ExecuteAsync(
        AppMenuItemModel item,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(item);
        await _executor.ExecuteAsync(item.Contribution, cancellationToken);
        UpdateSelection();
    }

    protected override void OnComponentActivated(CompositeDisposable disposables)
    {
        disposables.Add(_accessEvaluator.AccessChanged
            .Subscribe(_ => Recompose()));

        foreach (IAppMenuContribution contribution in _contributions)
        {
            disposables.Add(contribution.Changed
                .Subscribe(_ => Recompose()));
        }

        PropertyChangedEventHandler navigationChanged = (_, args) =>
        {
            if (args.PropertyName == nameof(AppNavigationState.Route))
            {
                UpdateSelection();
            }
        };
        _navigation.PropertyChanged += navigationChanged;
        disposables.Add(Disposable.Create(
            () => _navigation.PropertyChanged -= navigationChanged));

        Recompose();
    }

    private void Recompose()
    {
        AppMenuItemDescriptor[] descriptors = _contributions
            .Select(contribution => contribution.Descriptor.Validate())
            .ToArray();
        Dictionary<string, IAppMenuContribution> byId = _contributions
            .Zip(descriptors)
            .ToDictionary(pair => pair.Second.Id, pair => pair.First, StringComparer.Ordinal);

        foreach (AppMenuItemDescriptor descriptor in descriptors)
        {
            if (descriptor.ParentId is not null && !byId.ContainsKey(descriptor.ParentId))
            {
                throw new InvalidOperationException(
                    $"Menu contribution '{descriptor.Id}' references missing parent '{descriptor.ParentId}'.");
            }
        }
        ValidateHierarchy(descriptors);

        var composed = new List<AppMenuItemModel>();
        var path = new HashSet<string>(StringComparer.Ordinal);
        AppendChildren(parentId: null, depth: 0, byId, descriptors, path, composed);

        _items.Clear();
        foreach (AppMenuItemModel item in composed)
        {
            _items.Add(item);
        }

        UpdateSelection();
    }

    private void AppendChildren(
        string? parentId,
        int depth,
        IReadOnlyDictionary<string, IAppMenuContribution> byId,
        IEnumerable<AppMenuItemDescriptor> descriptors,
        ISet<string> path,
        ICollection<AppMenuItemModel> target)
    {
        foreach (AppMenuItemDescriptor descriptor in descriptors
                     .Where(item => item.ParentId == parentId)
                     .OrderBy(item => item.Order)
                     .ThenBy(item => item.Id, StringComparer.Ordinal))
        {
            if (!path.Add(descriptor.Id))
            {
                throw new InvalidOperationException($"Menu contribution cycle detected at '{descriptor.Id}'.");
            }

            IAppMenuContribution contribution = byId[descriptor.Id];
            if (_accessEvaluator.CheckAccess(contribution))
            {
                target.Add(new AppMenuItemModel(contribution, depth));
                AppendChildren(descriptor.Id, depth + 1, byId, descriptors, path, target);
            }

            path.Remove(descriptor.Id);
        }
    }

    private static void ValidateHierarchy(IReadOnlyCollection<AppMenuItemDescriptor> descriptors)
    {
        Dictionary<string, string?> parents = descriptors.ToDictionary(
            descriptor => descriptor.Id,
            descriptor => descriptor.ParentId,
            StringComparer.Ordinal);

        foreach (string id in parents.Keys)
        {
            var path = new HashSet<string>(StringComparer.Ordinal);
            string? current = id;
            while (current is not null)
            {
                if (!path.Add(current))
                {
                    throw new InvalidOperationException(
                        $"Menu contribution cycle detected at '{current}'.");
                }

                current = parents[current];
            }
        }
    }

    private void UpdateSelection()
    {
        foreach (AppMenuItemModel item in _items)
        {
            item.IsSelected = item.Id == _navigation.Route
                || _navigation.Route.StartsWith($"{item.Id}.", StringComparison.Ordinal);
        }

        _selectedItem = _items.FirstOrDefault(item => item.Id == _navigation.Route);
        this.RaisePropertyChanged(nameof(SelectedItem));
    }
}
