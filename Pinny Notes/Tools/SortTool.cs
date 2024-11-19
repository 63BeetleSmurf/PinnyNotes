using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Controls;

namespace Pinny_Notes.Tools;

public partial class SortTool : BaseTool, ITool
{
    public enum ToolActions
    {
        SortAscending,
        SortDescending
    }

    public SortTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "Sort";
        _menuActions.Add(new("Ascending", MenuActionCommand, ToolActions.SortAscending));
        _menuActions.Add(new("Descending", MenuActionCommand, ToolActions.SortDescending));
    }

    [RelayCommand]
    private void MenuAction(ToolActions action)
    {
        ApplyFunctionToNoteText(ModifyTextCallback, action);
    }

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
