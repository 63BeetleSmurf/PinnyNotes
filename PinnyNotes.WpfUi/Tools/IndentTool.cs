using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class IndentTool : BaseTool, ITool
{
    private enum ToolActions
    {
        Indent2Spaces,
        Indent4Spaces,
        IndentTab
    }

    public ToolStates State => ToolSettings.IndentToolState;

    public IndentTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Indent",
            [
                new ToolMenuAction("2 Spaces", new RelayCommand(() => MenuAction(ToolActions.Indent2Spaces))),
                new ToolMenuAction("4 Spaces", new RelayCommand(() => MenuAction(ToolActions.Indent4Spaces))),
                new ToolMenuAction("Tab", new RelayCommand(() => MenuAction(ToolActions.IndentTab)))
            ]
        );
    }

    private void MenuAction(ToolActions action)
    {
        ApplyFunctionToEachLine(ModifyLineCallback, action);
    }

    private string? ModifyLineCallback(string line, int index, Enum action)
    {
        return action switch
        {
            ToolActions.Indent2Spaces => $"  {line}",
            ToolActions.Indent4Spaces => $"    {line}",
            ToolActions.IndentTab => $"\t{line}",
            _ => line,
        };
    }
}
