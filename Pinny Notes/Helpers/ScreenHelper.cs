using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Pinny_Notes.Helpers;

public static class ScreenHelper
{
    public static nint GetWindowHandle(Window window)
    {
        return new WindowInteropHelper(window).Handle;
    }

    public static Rectangle GetCurrentScreenBounds(nint windowHandle)
    {
        Screen currentScreen = Screen.FromHandle(windowHandle);
        return currentScreen.Bounds;
    }

    public static Rectangle GetPrimaryScreenBounds()
    {
        return Screen.PrimaryScreen?.Bounds ??
            new Rectangle(0, 0, (int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight);
    }

}
