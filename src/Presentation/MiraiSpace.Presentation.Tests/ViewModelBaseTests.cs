using System.Reactive.Disposables;
using MiraiSpace.Presentation.Abstractions.Lifecycle;
using MiraiSpace.Presentation.ViewModels;

namespace MiraiSpace.Presentation.Tests;

public sealed class ViewModelBaseTests
{
    [Fact]
    public void DeactivationDisposesActivationResourcesAndCallsHook()
    {
        var viewModel = new TestViewModel();
        IDisposable activation = viewModel.Activator.Activate();

        activation.Dispose();

        Assert.True(viewModel.ResourceDisposed);
        Assert.True(viewModel.Deactivated);
    }

    [Fact]
    public async Task NewInitializationCancelsThePreviousRequest()
    {
        var viewModel = new TestViewModel();
        Task first = viewModel.InitializeAsync(1).AsTask();
        await viewModel.FirstStarted.Task;

        await viewModel.InitializeAsync(2);
        await first;

        Assert.Equal(2, viewModel.Value);
        Assert.True(viewModel.FirstCancelled);
    }

    [Fact]
    public void ComponentAndPageAreSemanticEmptyBases()
    {
        Assert.Equal(typeof(ViewModelBase), typeof(Component).BaseType);
        Assert.Equal(typeof(ViewModelBase), typeof(Page).BaseType);
    }

    private sealed class TestViewModel : ViewModelBase, IInitializable<int>
    {
        public TaskCompletionSource FirstStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public bool ResourceDisposed { get; private set; }
        public bool Deactivated { get; private set; }
        public bool FirstCancelled { get; private set; }
        public int Value { get; private set; }

        public ValueTask InitializeAsync(int parameter, CancellationToken cancellationToken = default) =>
            InitializeLatestAsync(async token =>
            {
                if (parameter == 1)
                {
                    FirstStarted.SetResult();
                    try { await Task.Delay(Timeout.Infinite, token); }
                    catch (OperationCanceledException) { FirstCancelled = true; }
                    return;
                }

                Value = parameter;
            }, cancellationToken);

        protected override void OnActivated(CompositeDisposable disposables) =>
            disposables.Add(Disposable.Create(() => ResourceDisposed = true));

        protected override void OnDeactivated() => Deactivated = true;
    }
}
