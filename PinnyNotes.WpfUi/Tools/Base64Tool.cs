using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public class Base64Tool : BaseTool, ITool
{
    private enum ToolActions
    {
        Base64Encode,
        Base64Decode
    }

    public ToolStates State => (ToolStates)Settings.Default.Base64ToolState;

    public Base64Tool(TextBox noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Base64",
            [
                new ToolMenuAction("Encode", new RelayCommand(() => MenuAction(ToolActions.Base64Encode))),
                new ToolMenuAction("Decode", new RelayCommand(() => MenuAction(ToolActions.Base64Decode)))
            ]
        );
    }

    private void MenuAction(Enum action)
    {
        ApplyFunctionToNoteText(ModifyTextCallback, action);
    }

    private string ModifyTextCallback(string text, Enum action)
    {
        switch (action)
        {
            case ToolActions.Base64Encode:
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text);
                return System.Convert.ToBase64String(textBytes);
            case ToolActions.Base64Decode:
                byte[] base64Bytes = System.Convert.FromBase64String(text);
                return System.Text.Encoding.UTF8.GetString(base64Bytes);
        }

        return text;
    }
}
