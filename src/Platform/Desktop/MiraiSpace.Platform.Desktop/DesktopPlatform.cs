namespace MiraiSpace.Platform.Desktop;

/// <summary>Exposes the operating-system module selected for this desktop build.</summary>
public static class DesktopPlatform
{
    public static Type ModuleType
    {
        get
        {
#if WINDOWS
            return typeof(global::MiraiSpace.Platform.Windows.WindowsPlatformModule);
#elif LINUX
            return typeof(global::MiraiSpace.Platform.Linux.LinuxPlatformModule);
#elif MACOS
            return typeof(global::MiraiSpace.Platform.MacOS.MacOSPlatformModule);
#else
            return typeof(DesktopPlatform);
#endif
        }
    }
}
