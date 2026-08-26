using MiraiSpace.Presentation.ViewModels;

namespace MiraiSpace.Presentation.Tests;

public sealed class ViewModelBaseTests
{
    [Fact]
    public void DisposeDisposesOwnedResourcesOnce()
    {
        var resource = new RecordingDisposable();
        var viewModel = new TestViewModel(resource);

        viewModel.Dispose();
        viewModel.Dispose();

        Assert.Equal(1, resource.DisposeCount);
    }

    private sealed class TestViewModel : ViewModelBase
    {
        public TestViewModel(IDisposable resource)
        {
            Own(resource);
        }
    }

    private sealed class RecordingDisposable : IDisposable
    {
        public int DisposeCount { get; private set; }

        public void Dispose() => DisposeCount++;
    }
}
