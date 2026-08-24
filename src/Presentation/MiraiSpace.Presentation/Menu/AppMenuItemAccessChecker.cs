using MiraiSpace.Extensibility.Abstractions.Menu;

namespace MiraiSpace.Presentation.Menu;

public sealed class AppMenuItemAccessChecker : IAppMenuItemAccessChecker, IDisposable
{
    private readonly IReadOnlyList<IAppMenuItemAccessPolicy> _policies;

    public AppMenuItemAccessChecker(IEnumerable<IAppMenuItemAccessPolicy> policies)
    {
        _policies = policies.ToArray();
        foreach (IAppMenuItemAccessPolicy policy in _policies)
        {
            policy.AccessChanged += OnPolicyAccessChanged;
        }
    }

    public event EventHandler? AccessChanged;

    public bool CheckAccess(IAppMenuItem item) =>
        _policies.All(policy => policy.CheckAccess(item));

    public void Dispose()
    {
        foreach (IAppMenuItemAccessPolicy policy in _policies)
        {
            policy.AccessChanged -= OnPolicyAccessChanged;
        }
    }

    private void OnPolicyAccessChanged(object? sender, EventArgs e) =>
        AccessChanged?.Invoke(this, EventArgs.Empty);
}
