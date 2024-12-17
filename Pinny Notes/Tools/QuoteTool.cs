using System;
using System.Linq;
using System.Windows.Controls;

namespace PinnyNotes.WpfUi.Tools;

public static class QuoteTool
{
    private static char[] _openingQuotes = { '\'', '"', '`', '‘', '“' };
    private static char[] _closingQuotes = { '\'', '"', '`', '’', '”' };

    public const string Name = "Quote";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("Double", OnDoubleMenuItemClick),
                new("Single", OnSingleMenuItemClick),
                new("Backtick", OnBacktickMenuItemClick),
                new("-"),
                new("Trim", OnTrimMenuItemClick)
            ]
        );

    private static void OnDoubleMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), AddDouble);
    private static void OnSingleMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), AddSingle);
    private static void OnBacktickMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), AddBacktick);
    private static void OnTrimMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), TrimQuotes);

    private static string? AddDouble(string line, int index)
        => $"\"{line}\"";

    private static string? AddSingle(string line, int index)
        => $"'{line}'";

    private static string? AddBacktick(string line, int index)
        => $"`{line}`";

    private static string? TrimQuotes(string line, int index)
    {
        if (line.Length > 0 && _openingQuotes.Contains(line[0]) && _closingQuotes.Contains(line[^1]))
            return line[1..^1];

        return line;
    }
}
