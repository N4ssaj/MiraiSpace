using Avalonia.Threading;
using MiraiSpace.Presentation.Foundation;

namespace MiraiSpace.UI.Infrastructure;

public sealed class UiDispatcher : IUiDispatcher
{
    public Task<T> InvokeAsync<T>(Func<Task<T>> action) => Dispatcher.UIThread.InvokeAsync(action);
}
