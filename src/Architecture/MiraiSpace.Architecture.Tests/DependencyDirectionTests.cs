using System.Reflection;
using MiraiSpace.Presentation.ViewModels;

namespace MiraiSpace.Architecture.Tests;

public sealed class DependencyDirectionTests
{
    [Fact]
    public void CoreHasNoOutgoingMiraiSpaceDependency() =>
        Assert.Empty(MiraiSpaceReferences(Assembly.Load("MiraiSpace.Core")));

    [Fact]
    public void ApplicationOnlyDependsOnCore() =>
        Assert.All(
            MiraiSpaceReferences(Assembly.Load("MiraiSpace.Application")),
            name => Assert.Equal("MiraiSpace.Core", name));

    [Fact]
    public void PresentationAbstractionsOnlyReferenceTheBcl()
    {
        string[] references = References(Assembly.Load("MiraiSpace.Presentation.Abstractions"));

        Assert.DoesNotContain(references, name => name.StartsWith("MiraiSpace.", StringComparison.Ordinal));
        Assert.DoesNotContain(references, name => name.StartsWith("Reactive", StringComparison.Ordinal));
        Assert.DoesNotContain(references, name => name.StartsWith("DynamicData", StringComparison.Ordinal));
        Assert.DoesNotContain(references, name => name.StartsWith("Avalonia", StringComparison.Ordinal));
        Assert.DoesNotContain(references, name => name.StartsWith("Eremex", StringComparison.Ordinal));
        Assert.DoesNotContain(references, name => name.StartsWith("Microsoft.Extensions", StringComparison.Ordinal));
    }

    [Fact]
    public void PresentationDoesNotReferenceUiOrInfrastructure()
    {
        string[] references = References(typeof(ViewModelBase).Assembly);

        Assert.DoesNotContain(references, name => name.StartsWith("Avalonia", StringComparison.Ordinal));
        Assert.DoesNotContain(references, name => name.StartsWith("Eremex", StringComparison.Ordinal));
        Assert.DoesNotContain(references, name => name.StartsWith("MiraiSpace.UI", StringComparison.Ordinal));
        Assert.DoesNotContain(references, name => name.StartsWith("MiraiSpace.Infrastructure", StringComparison.Ordinal));
    }

    [Fact]
    public void InfrastructureDoesNotReferencePresentationOrUi()
    {
        string[] references = References(Assembly.Load("MiraiSpace.Infrastructure.Api"));

        Assert.DoesNotContain(references, name => name.StartsWith("MiraiSpace.Presentation", StringComparison.Ordinal));
        Assert.DoesNotContain(references, name => name.StartsWith("MiraiSpace.UI", StringComparison.Ordinal));
        Assert.DoesNotContain(references, name => name.StartsWith("Avalonia", StringComparison.Ordinal));
    }

    private static string[] MiraiSpaceReferences(Assembly assembly) =>
        References(assembly)
            .Where(name => name.StartsWith("MiraiSpace.", StringComparison.Ordinal))
            .ToArray();

    private static string[] References(Assembly assembly) =>
        assembly.GetReferencedAssemblies()
            .Select(reference => reference.Name!)
            .ToArray();
}
