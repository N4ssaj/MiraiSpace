using System.ComponentModel;
using HanumanInstitute.MvvmDialogs;
using MiraiSpace.Presentation.Lifecycle;

namespace MiraiSpace.Presentation.Dialogs;

public interface IDialogService
{
    Task<bool?> ShowAsync<TDialog>(
        INotifyPropertyChanged owner,
        TDialog dialog,
        CancellationToken cancellationToken = default)
        where TDialog : class, IModalDialogViewModel, ICloseable;

    Task<bool?> ShowAsync<TDialog, TParameters>(
        INotifyPropertyChanged owner,
        TDialog dialog,
        TParameters parameters,
        CancellationToken cancellationToken = default)
        where TDialog : class, IModalDialogViewModel, ICloseable, IInitializable<TParameters>;
}
