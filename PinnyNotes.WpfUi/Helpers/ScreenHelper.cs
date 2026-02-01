using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

using PinnyNotes.WpfUi.Interop;
using PinnyNotes.WpfUi.Interop.Constants;
using PinnyNotes.WpfUi.Interop.Structures;

namespace PinnyNotes.WpfUi.Helpers;

public static class ScreenHelper
{
    public static nint GetWindowHandle(Window window)
    {
        return new WindowInteropHelper(window).Handle;
    }

    public static Rect GetCurrentScreenBounds(nint windowHandle)
    {
        MONITORINFO monitorInfo = new();
        monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);

        nint hMonitor = User32.MonitorFromWindow(windowHandle, MONITOR.DEFAULTTONEAREST);
        User32.GetMonitorInfoW(hMonitor, ref monitorInfo);

        return RectFromMonitorInfo(monitorInfo);
    }

    public static Rect GetPrimaryScreenBounds()
    {
        POINT pt = new() { x = 0, y = 0 };
        nint hMonitor = User32.MonitorFromPoint(pt, MONITOR.DEFAULTTOPRIMARY);

        MONITORINFO monitorInfo = new();
        monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);
        User32.GetMonitorInfoW(hMonitor, ref monitorInfo);

        return RectFromMonitorInfo(monitorInfo);
    }
    private static Rect RectFromMonitorInfo(MONITORINFO monitorInfo)
    {
        return new Rect
        {
            X = monitorInfo.rcMonitor.Left,
            Y = monitorInfo.rcMonitor.Top,
            Width = monitorInfo.rcMonitor.Right - monitorInfo.rcMonitor.Left,
            Height = monitorInfo.rcMonitor.Bottom - monitorInfo.rcMonitor.Top
        };
    }
}
