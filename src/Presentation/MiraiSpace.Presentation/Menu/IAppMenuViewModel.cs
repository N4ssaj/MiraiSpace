namespace MiraiSpace.Presentation.Menu;

public interface IAppMenuViewModel
{
    IReadOnlyList<AppMenuItemModel> Items { get; }

    AppMenuItemModel? SelectedItem { get; set; }

    ValueTask ExecuteAsync(AppMenuItemModel item, CancellationToken cancellationToken = default);
}
