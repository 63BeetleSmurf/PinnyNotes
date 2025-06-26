using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public class Base64Tool : BaseTool, ITool
{
    public bool IsEnabled => ToolSettings.Default.Base64ToolEnabled;
    public bool IsFavourite => ToolSettings.Default.Base64ToolFavourite;

    public enum ToolActions
    {
        Base64Encode,
        Base64Decode
    }

    public Base64Tool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "Base64";
        _menuActions.Add(new("Encode", new RelayCommand(() => MenuAction(ToolActions.Base64Encode))));
        _menuActions.Add(new("Decode", new RelayCommand(() => MenuAction(ToolActions.Base64Decode))));
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
