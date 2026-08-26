using Avalonia;
using Avalonia.Controls;

namespace MiraiSpace.UI.Views.Menu;

public static class MenuDisplay
{
    public static readonly AttachedProperty<bool> IsCompactProperty =
        AvaloniaProperty.RegisterAttached<Control, Control, bool>(
            "IsCompact",
            defaultValue: false,
            inherits: true);

    public static bool GetIsCompact(AvaloniaObject element) => element.GetValue(IsCompactProperty);
    public static void SetIsCompact(AvaloniaObject element, bool value) => element.SetValue(IsCompactProperty, value);
}
