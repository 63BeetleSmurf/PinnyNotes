using System;
using System.Windows.Controls;

namespace PinnyNotes.WpfUi.Tools;

public static class SortTool
{
    public const string Name = "Sort";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("Ascending", OnAscendingMenuItemClick),
                new("Descending", OnDescendingMenuItemClick)
            ]
        );

    private static void OnAscendingMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), SortAscending);
    private static void OnDescendingMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), SortDescending);

    private static string SortAscending(string text, object? additionalParam)
        => SortNoteText(text);

    private static string SortDescending(string text, object? additionalParam)
        => SortNoteText(text, true);

    private static string SortNoteText(string text, bool reverse = false)
    {
        string[] lines = text.Split(Environment.NewLine);
        Array.Sort(lines);
        if (reverse)
            Array.Reverse(lines);
        return string.Join(Environment.NewLine, lines);
    }
}
