using System;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class UrlTool : BaseTool, ITool
{
    private enum ToolActions
    {
        Encode,
        Decode
    }
    public ToolStates State => ToolSettings.UrlToolState;

    public UrlTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "URL",
            [
                new ToolMenuAction("Encode", new RelayCommand(() => MenuAction(ToolActions.Encode))),
                new ToolMenuAction("Decode", new RelayCommand(() => MenuAction(ToolActions.Decode)))
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
            ToolActions.Encode => Uri.EscapeDataString(text),
            ToolActions.Decode => Uri.UnescapeDataString(text),
            _ => text,
        };
    }
}
