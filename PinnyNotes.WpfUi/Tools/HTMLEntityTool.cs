using System;
using System.Net;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class HtmlEntityTool : BaseTool, ITool
{
    private enum ToolActions
    {
        EntityEncode,
        EntityDecode
    }
    public ToolStates State => ToolSettings.HtmlEntityToolState;

    public HtmlEntityTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "HTML Entity",
            [
                new ToolMenuAction("Encode", new RelayCommand(() => MenuAction(ToolActions.EntityEncode))),
                new ToolMenuAction("Decode", new RelayCommand(() => MenuAction(ToolActions.EntityDecode)))
            ]
        );
    }

    private void MenuAction(ToolActions action)
    {
        ApplyFunctionToNoteText(ModifyTextCallback, action);
    }

    private string ModifyTextCallback(string text, Enum action)
    {
        return action switch
        {
            ToolActions.EntityEncode => WebUtility.HtmlEncode(text),
            ToolActions.EntityDecode => WebUtility.HtmlDecode(text),
            _ => text,
        };
    }
}
