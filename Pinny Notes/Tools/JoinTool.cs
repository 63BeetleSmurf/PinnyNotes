using System;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Tools;

public partial class JoinTool : BaseTool, ITool
{
    public ToolStates State => (ToolStates)ToolSettings.Default.JoinToolState;

    public enum ToolActions
    {
        JoinComma,
        JoinSpace,
        JoinTab
    }

    public JoinTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        _name = "Join";
        _menuActions.Add(new("Comma", JoinCommaMenuAction));
        _menuActions.Add(new("Space", JoinSpaceMenuAction));
        _menuActions.Add(new("Tab", JoinTabMenuAction));
    }

    private void JoinCommaMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.JoinComma);
    private void JoinSpaceMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.JoinSpace);
    private void JoinTabMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.JoinTab);

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
