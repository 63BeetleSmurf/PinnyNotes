using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;

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

    public ToolStates State => (ToolStates)Settings.Default.TrimToolState;

    public TrimTool(TextBox noteTextBox) : base(noteTextBox)
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
