using System;
using System.Net;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public class HtmlEntityTool : BaseTool, ITool
{
    public ToolStates State => (ToolStates)Settings.Default.HtmlEntityToolState;

    public enum ToolActions
    {
        EntityEncode,
        EntityDecode
    }

    public HtmlEntityTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "HTML Entity";
        _menuActions.Add(new("Encode", new RelayCommand(() => MenuAction(ToolActions.EntityEncode))));
        _menuActions.Add(new("Decode", new RelayCommand(() => MenuAction(ToolActions.EntityDecode))));
    }

    private void MenuAction(ToolActions action)
    {
        ApplyFunctionToNoteText(ModifyTextCallback, action);
    }

    private string ModifyTextCallback(string text, Enum action)
    {
        switch (action)
        {
            case ToolActions.EntityEncode:
                return WebUtility.HtmlEncode(text);
            case ToolActions.EntityDecode:
                return WebUtility.HtmlDecode(text);
        }

        return text;
    }
}
