using System;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Controls;
using PinnyNotes.WpfUi.Commands;

namespace PinnyNotes.WpfUi.Tools;

public class JoinTool : BaseTool, ITool
{
    private enum ToolActions
    {
        JoinComma,
        JoinSpace,
        JoinTab
    }

    public ToolStates State => _settings.ToolSettings.JoinToolState;

    public JoinTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Join",
            [
                new ToolMenuAction("Comma", new RelayCommand(() => MenuAction(ToolActions.JoinComma))),
                new ToolMenuAction("Space", new RelayCommand(() => MenuAction(ToolActions.JoinSpace))),
                new ToolMenuAction("Tab", new RelayCommand(() => MenuAction(ToolActions.JoinTab)))
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
            case ToolActions.JoinComma:
                return text.Replace(Environment.NewLine, ",");
            case ToolActions.JoinSpace:
                return text.Replace(Environment.NewLine, " ");
            case ToolActions.JoinTab:
                return text.Replace(Environment.NewLine, "\t");
        }

        return text;
    }
}
