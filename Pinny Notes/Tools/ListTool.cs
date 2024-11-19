using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Controls;

namespace Pinny_Notes.Tools;

public partial class ListTool : BaseTool, ITool
{
    public enum ToolActions
    {
        ListEnumerate,
        ListDash,
        ListRemove
    }

    public ListTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "List";
        _menuActions.Add(new("Enumerate", MenuActionCommand, ToolActions.ListEnumerate));
        _menuActions.Add(new("Dash", MenuActionCommand, ToolActions.ListDash));
        _menuActions.Add(new("Remove", MenuActionCommand, ToolActions.ListRemove));
    }

    [RelayCommand]
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
