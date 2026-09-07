using Avalonia;
using Avalonia.Markup.Xaml;

namespace MiraiSpace.UI;

public partial class App : Avalonia.Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
