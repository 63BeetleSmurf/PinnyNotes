using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public class JoinTool : BaseTool, ITool
{
    private enum ToolActions
    {
        JoinComma,
        JoinSpace,
        JoinTab
    }

    public ToolStates State => (ToolStates)Settings.Default.JoinToolState;

    public JoinTool(TextBox noteTextBox) : base(noteTextBox)
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
