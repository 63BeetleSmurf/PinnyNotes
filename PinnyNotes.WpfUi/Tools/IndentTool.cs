using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public class IndentTool : BaseTool, ITool
{
    public ToolStates State => (ToolStates)Settings.Default.IndentToolState;

    public enum ToolActions
    {
        Indent2Spaces,
        Indent4Spaces,
        IndentTab
    }

    public IndentTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "Indent";
        _menuActions.Add(new("2 Spaces", new RelayCommand(() => MenuAction(ToolActions.Indent2Spaces))));
        _menuActions.Add(new("4 Spaces", new RelayCommand(() => MenuAction(ToolActions.Indent4Spaces))));
        _menuActions.Add(new("Tab", new RelayCommand(() => MenuAction(ToolActions.IndentTab))));
    }

    private void MenuAction(ToolActions action)
    {
        ApplyFunctionToEachLine(ModifyLineCallback, action);
    }

    private string? ModifyLineCallback(string line, int index, Enum action)
    {
        switch (action)
        {
            case ToolActions.Indent2Spaces:
                return $"  {line}";
            case ToolActions.Indent4Spaces:
                return $"    {line}";
            case ToolActions.IndentTab:
                return $"\t{line}";
        }

        return line;
    }
}
