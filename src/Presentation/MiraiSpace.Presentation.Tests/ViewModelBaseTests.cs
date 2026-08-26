using System.Reactive.Disposables;
using MiraiSpace.Presentation.Abstractions.Lifecycle;
using MiraiSpace.Presentation.ViewModels;

namespace MiraiSpace.Presentation.Tests;

public sealed class ViewModelBaseTests
{
    [Fact]
    public void ActivationAndDeactivationHooksFollowTheActivator()
    {
        using var viewModel = new RecordingViewModel();

        IDisposable activation = viewModel.Activator.Activate();
        activation.Dispose();

        Assert.Equal(1, viewModel.ActivationCount);
        Assert.Equal(1, viewModel.DeactivationCount);
    }

    [Fact]
    public async Task NewInitializationCancelsThePreviousInput()
    {
        using var viewModel = new ParameterViewModel();
        Task first = viewModel.InitializeAsync("first").AsTask();

        await viewModel.InitializeAsync("second");
        await first;

        Assert.True(viewModel.FirstWasCancelled);
        Assert.Equal("second", viewModel.Value);
    }

    [Fact]
    public void ComponentAndPageRemainSemanticEmptyBases()
    {
        Assert.Equal(typeof(ViewModelBase), typeof(Component).BaseType);
        Assert.Equal(typeof(ViewModelBase), typeof(Page).BaseType);
    }

    private sealed class RecordingViewModel : ViewModelBase
    {
        public int ActivationCount { get; private set; }
        public int DeactivationCount { get; private set; }

        protected override void OnActivated(CompositeDisposable disposables) => ActivationCount++;
        protected override void OnDeactivated() => DeactivationCount++;
    }

    private sealed class ParameterViewModel : ViewModelBase, IInitializable<string>
    {
        private readonly TaskCompletionSource _firstStarted = new(
            TaskCreationOptions.RunContinuationsAsynchronously);

        public bool FirstWasCancelled { get; private set; }
        public string? Value { get; private set; }

        public async ValueTask InitializeAsync(
            string parameter,
            CancellationToken cancellationToken = default) =>
            await ReinitializeAsync(async token =>
            {
                if (parameter == "first")
                {
                    _firstStarted.SetResult();
                    try
                    {
                        await Task.Delay(Timeout.InfiniteTimeSpan, token);
                    }
                    catch (OperationCanceledException)
                    {
                        FirstWasCancelled = true;
                    }
                    return;
                }

                await _firstStarted.Task;
                Value = parameter;
            }, cancellationToken);
    }
}
