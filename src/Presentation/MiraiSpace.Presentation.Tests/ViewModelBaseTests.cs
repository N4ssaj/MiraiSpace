using System.Reactive.Disposables;
using MiraiSpace.Presentation.ViewModels;

namespace MiraiSpace.Presentation.Tests;

public sealed class ViewModelBaseTests
{
    [Fact]
    public void ActivationOwnsResourcesUntilViewDeactivates()
    {
        var resource = new RecordingDisposable();
        var viewModel = new TestViewModel(resource);

        IDisposable activation = viewModel.Activator.Activate();
        Assert.Equal(0, resource.DisposeCount);

        activation.Dispose();
        Assert.Equal(1, resource.DisposeCount);
    }

    [Fact]
    public void ModelDoesNotExposeAViewActivator()
    {
        Assert.False(typeof(ReactiveUI.IActivatableViewModel).IsAssignableFrom(typeof(TestModel)));
    }

    [Fact]
    public void PageActivationCancelsPageWorkWhenViewDeactivates()
    {
        var page = new TestPage();

        IDisposable activation = page.Activator.Activate();
        Assert.False(page.ActivationToken.IsCancellationRequested);

        activation.Dispose();
        Assert.True(page.ActivationToken.IsCancellationRequested);
    }

    private sealed class TestViewModel(IDisposable resource) : ViewModelBase
    {
        protected override void OnActivated(CompositeDisposable disposables) =>
            disposables.Add(resource);
    }

    private sealed class TestModel : ModelBase;

    private sealed class TestPage : PageViewModelBase
    {
        public TestPage() : base("test", "Test")
        {
        }

        public CancellationToken ActivationToken { get; private set; }

        protected override void OnPageActivated(
            CompositeDisposable disposables,
            CancellationToken cancellationToken) =>
            ActivationToken = cancellationToken;
    }

    private sealed class RecordingDisposable : IDisposable
    {
        public int DisposeCount { get; private set; }
        public void Dispose() => DisposeCount++;
    }
}
