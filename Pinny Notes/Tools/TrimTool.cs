using System;
using System.Windows.Controls;

namespace PinnyNotes.WpfUi.Tools;

public static class TrimTool
{
    public const string Name = "Trim";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("Start", OnStartMenuItemClick),
                new("End", OnEndMenuItemClick),
                new("Both", OnBothMenuItemClick),
                new("Empty Lines", OnLinesMenuItemClick)
            ]
        );

    private static void OnStartMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), TrimStart);
    private static void OnEndMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), TrimEnd);
    private static void OnBothMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), TrimBoth);
    private static void OnLinesMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), TrimEmptyLines);

    private static string? TrimStart(string line, int index)
        => line.TrimStart();

    private static string? TrimEnd(string line, int index)
        => line.TrimEnd();

    private static string? TrimBoth(string line, int index)
        => line.Trim();

    private static string? TrimEmptyLines(string line, int index)
    {
        if (string.IsNullOrEmpty(line))
            return null;

            return line;
    }
}
