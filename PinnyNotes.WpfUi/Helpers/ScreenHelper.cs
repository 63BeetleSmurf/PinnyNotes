using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace PinnyNotes.WpfUi.Helpers;

public static partial class ScreenHelper
{
    public static nint GetWindowHandle(Window window)
    {
        return new WindowInteropHelper(window).Handle;
    }

    public static Rect GetCurrentScreenBounds(nint windowHandle)
    {
        const int MONITOR_DEFAULTTONEAREST = 2;

        MONITORINFO monitorInfo = new();
        monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);

        nint hMonitor = MonitorFromWindow(windowHandle, MONITOR_DEFAULTTONEAREST);
        GetMonitorInfoW(hMonitor, ref monitorInfo);

        return RectFromMonitorInfo(monitorInfo);
    }

    public static Rect GetPrimaryScreenBounds()
    {
        const int MONITOR_DEFAULTTOPRIMARY = 1;

        POINT pt = new() { x = 0, y = 0 };
        nint hMonitor = MonitorFromPoint(pt, MONITOR_DEFAULTTOPRIMARY);

        MONITORINFO monitorInfo = new();
        monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);
        GetMonitorInfoW(hMonitor, ref monitorInfo);

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

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MONITORINFO
    {
        public int cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
    }

    [LibraryImport("user32.dll")]
    private static partial nint MonitorFromWindow(nint hwnd, int dwFlags);

    [LibraryImport("user32.dll")]
    private static partial nint MonitorFromPoint(POINT pt, int dwFlags);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetMonitorInfoW(nint hMonitor, ref MONITORINFO lpmi);
}
