namespace MiraiSpace.Presentation.Foundation;

public interface IUiDispatcher
{
    Task<T> InvokeAsync<T>(Func<Task<T>> action);
}
