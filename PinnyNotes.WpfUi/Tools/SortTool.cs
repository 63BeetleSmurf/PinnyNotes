using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public class SortTool : BaseTool, ITool
{
    public bool IsEnabled => ToolSettings.Default.SortToolEnabled;
    public bool IsFavourite => ToolSettings.Default.SortToolFavourite;

    public enum ToolActions
    {
        SortAscending,
        SortDescending
    }

    public SortTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "Sort";
        _menuActions.Add(new("Ascending", new RelayCommand(() => MenuAction(ToolActions.SortAscending))));
        _menuActions.Add(new("Descending", new RelayCommand(() => MenuAction(ToolActions.SortDescending))));
    }

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
