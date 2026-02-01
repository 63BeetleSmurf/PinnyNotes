using System.Runtime.InteropServices;

using PinnyNotes.WpfUi.Interop.Structures;

namespace PinnyNotes.WpfUi.Interop;

internal partial class User32
{
    [LibraryImport("user32.dll")]
    public static partial nint MonitorFromWindow(nint hwnd, int dwFlags);

    [LibraryImport("user32.dll")]
    public static partial nint MonitorFromPoint(POINT pt, int dwFlags);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetMonitorInfoW(nint hMonitor, ref MONITORINFO lpmi);


    [LibraryImport("user32.dll")]
    public static partial int GetWindowLongPtrW(nint hWnd, int nIndex);

    [LibraryImport("user32.dll")]
    public static partial int SetWindowLongPtrW(nint hWnd, int nIndex, nint dwNewLong);

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetWindowPos(nint hWnd, nint hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
}
