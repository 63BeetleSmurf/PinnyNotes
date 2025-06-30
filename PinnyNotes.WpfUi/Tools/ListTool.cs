using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public class ListTool : BaseTool, ITool
{
    public ToolStates State => (ToolStates)Settings.Default.ListToolState;

    public enum ToolActions
    {
        ListEnumerate,
        ListDash,
        ListRemove
    }

    public ListTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "List";
        _menuActions.Add(new("Enumerate", new RelayCommand(() => MenuAction(ToolActions.ListEnumerate))));
        _menuActions.Add(new("Dash", new RelayCommand(() => MenuAction(ToolActions.ListDash))));
        _menuActions.Add(new("Remove", new RelayCommand(() => MenuAction(ToolActions.ListRemove))));
    }

    private void MenuAction(ToolActions action)
    {
        ApplyFunctionToEachLine(ModifyLineCallback, action);
    }

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
