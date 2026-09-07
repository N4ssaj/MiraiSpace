using Avalonia;
using Avalonia.Controls;

namespace MiraiSpace.UI.AttachedProperties;

public sealed class AppMenuDisplay
{
    public static readonly AttachedProperty<bool> IsCompactProperty =
        AvaloniaProperty.RegisterAttached<AppMenuDisplay, Control, bool>(
            "IsCompact",
            defaultValue: false,
            inherits: true);

    private AppMenuDisplay()
    {
    }

    public static bool GetIsCompact(Control control) =>
        control.GetValue(IsCompactProperty);

    public static void SetIsCompact(Control control, bool value) =>
        control.SetValue(IsCompactProperty, value);
}
