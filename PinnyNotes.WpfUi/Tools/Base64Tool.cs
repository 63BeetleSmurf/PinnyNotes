using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class Base64Tool : BaseTool, ITool
{
    private enum ToolActions
    {
        Base64Encode,
        Base64Decode
    }

    public Base64Tool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Base64",
            [
                new ToolMenuAction("Encode", new RelayCommand(() => MenuAction(ToolActions.Base64Encode))),
                new ToolMenuAction("Decode", new RelayCommand(() => MenuAction(ToolActions.Base64Decode)))
            ]
        );
    }

    public ToolState State => ToolSettings.Base64ToolState;

    private void MenuAction(Enum action)
    {
        ApplyFunctionToNoteText(ModifyTextCallback, action);
    }

    private string ModifyTextCallback(string text, Enum action)
    {
        try
        {
            switch (action)
            {
                case ToolActions.Base64Encode:
                    byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text);
                    return Convert.ToBase64String(textBytes);
                case ToolActions.Base64Decode:
                    byte[] base64Bytes = Convert.FromBase64String(text);
                    return System.Text.Encoding.UTF8.GetString(base64Bytes);
            }
        }
        catch
        {
        }

        return text;
    }
}
