using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public partial class TrimTool : BaseTool, ITool
{
    public ToolStates State => (ToolStates)ToolSettings.Default.TrimToolState;

    public enum ToolActions
    {
        TrimStart,
        TrimEnd,
        TrimBoth,
        TrimLines
    }

    public TrimTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "Trim";
        _menuActions.Add(new("Start", TrimStartMenuAction));
        _menuActions.Add(new("End", TrimEndMenuAction));
        _menuActions.Add(new("Both", TrimBothMenuAction));
        _menuActions.Add(new("Empty Lines", TrimLinesMenuAction));
    }

    private void TrimStartMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.TrimStart);
    private void TrimEndMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.TrimEnd);
    private void TrimBothMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.TrimBoth);
    private void TrimLinesMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.TrimLines);

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
