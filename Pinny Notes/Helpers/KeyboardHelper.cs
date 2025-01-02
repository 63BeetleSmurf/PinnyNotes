using System.Windows.Input;

namespace PinnyNotes.WpfUi.Helpers;

public static class KeyboardHelper
{
    public static bool IsShiftPressed()
        => ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift);

    public static bool IsControlPressed()
        => ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control);
}
