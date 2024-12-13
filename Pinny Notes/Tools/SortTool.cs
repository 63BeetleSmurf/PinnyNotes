using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Enums;

namespace PinnyNotes.WpfUi.Tools;

public partial class SortTool : BaseTool, ITool
{
    public ToolStates State => ToolStates.Disabled; // (ToolStates)ToolSettings.Default.SortToolState;

    public enum ToolActions
    {
        SortAscending,
        SortDescending
    }

    public SortTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "Sort";
        _menuActions.Add(new("Ascending", SortAscendingMenuAction));
        _menuActions.Add(new("Descending", SortDescendingMenuAction));
    }

    private void SortAscendingMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.SortAscending);
    private void SortDescendingMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.SortDescending);

    private string ModifyTextCallback(string text, Enum action)
    {
        switch (action)
        {
            case ToolActions.SortAscending:
                return SortNoteText(text);
            case ToolActions.SortDescending:
                return SortNoteText(text, true);
        }

        return text;
    }

    private string SortNoteText(string text, bool reverse = false)
    {
        string[] lines = text.Split(Environment.NewLine);
        Array.Sort(lines);
        if (reverse)
            Array.Reverse(lines);
        return string.Join(Environment.NewLine, lines);
    }
}
