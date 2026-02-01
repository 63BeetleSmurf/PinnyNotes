using System.Runtime.InteropServices;

namespace PinnyNotes.WpfUi.Interop.Structures;

[StructLayout(LayoutKind.Sequential)]
public struct RECT
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;
}
