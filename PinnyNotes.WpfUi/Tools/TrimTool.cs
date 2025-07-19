using System;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class TrimTool : BaseTool, ITool
{
    private enum ToolActions
    {
        TrimStart,
        TrimEnd,
        TrimBoth,
        TrimLines
    }

    public ToolStates State => ToolSettings.TrimToolState;

    public TrimTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Trim",
            [
                new ToolMenuAction("Start", new RelayCommand(() => MenuAction(ToolActions.TrimStart))),
                new ToolMenuAction("End", new RelayCommand(() => MenuAction(ToolActions.TrimEnd))),
                new ToolMenuAction("Both", new RelayCommand(() => MenuAction(ToolActions.TrimBoth))),
                new ToolMenuAction("Empty Lines", new RelayCommand(() => MenuAction(ToolActions.TrimLines)))
            ]
        );
    }

    private void MenuAction(ToolActions action)
    {
        ApplyFunctionToEachLine(ModifyLineCallback, action);
    }

    private string? ModifyLineCallback(string line, int index, Enum action)
    {
        switch (action)
        {
            case ToolActions.TrimStart:
                return line.TrimStart();
            case ToolActions.TrimEnd:
                return line.TrimEnd();
            case ToolActions.TrimBoth:
                return line.Trim();
            case ToolActions.TrimLines:
                if (string.IsNullOrEmpty(line))
                    return null;
                else
                    return line;
        }

        return line;
    }
}
