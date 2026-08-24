namespace MiraiSpace.Extensibility.Abstractions;

public interface IRoleRestricted
{
    /// <summary>
    /// Gets the roles that the current user must all possess. An empty collection allows access.
    /// </summary>
    IReadOnlyCollection<Guid> RequiredRoleIds { get; }
}
