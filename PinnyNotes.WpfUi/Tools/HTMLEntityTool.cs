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
    public ToolStates State => _settings.ToolSettings.HtmlEntityToolState;

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
