using System;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Tools;

public partial class ListTool : BaseTool, ITool
{
    public ToolStates State => (ToolStates)ToolSettings.Default.ListToolState;

    public enum ToolActions
    {
        ListEnumerate,
        ListDash,
        ListRemove
    }

    public ListTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        _name = "List";
        _menuActions.Add(new("Enumerate", ListEnumerateMenuAction));
        _menuActions.Add(new("Dash", ListDashMenuAction));
        _menuActions.Add(new("Remove", ListRemoveMenuAction));
    }

    private void ListEnumerateMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.ListEnumerate);
    private void ListDashMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.ListDash);
    private void ListRemoveMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.ListRemove);

    private string? ModifyLineCallback(string line, int index, Enum action)
    {
        switch (action)
        {
            case ToolActions.ListEnumerate:
                return $"{index + 1}. {line}";
            case ToolActions.ListDash:
                return $"- {line}";
            case ToolActions.ListRemove:
                return line[(line.IndexOf(' ') + 1)..];
        }

        return line;
    }
}
