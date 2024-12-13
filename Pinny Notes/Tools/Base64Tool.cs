using System;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Tools;

public partial class Base64Tool : BaseTool, ITool
{
    public ToolStates State => (ToolStates)ToolSettings.Default.Base64ToolState;

    public enum ToolActions
    {
        Base64Encode,
        Base64Decode
    }

    public Base64Tool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        _name = "Base64";
        _menuActions.Add(new("Encode", Base64EncodeMenuAction));
        _menuActions.Add(new("Decode", Base64DecodeMenuAction));
    }

    private void Base64EncodeMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.Base64Encode);
    private void Base64DecodeMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.Base64Decode);

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
