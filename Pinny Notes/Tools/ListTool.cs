using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public partial class ListTool : BaseTool, ITool
{
    public bool IsEnabled => ToolSettings.Default.ListToolEnabled;
    public bool IsFavourite => ToolSettings.Default.ListToolFavourite;

    public enum ToolActions
    {
        ListEnumerate,
        ListDash,
        ListRemove
    }

    public ListTool(TextBox noteTextBox) : base(noteTextBox)
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
