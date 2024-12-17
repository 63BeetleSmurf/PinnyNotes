using System;
using System.Linq;
using System.Windows.Controls;

namespace PinnyNotes.WpfUi.Tools;

public static class BracketTool
{
    private static char[] _openingBrackets = {'(', '{', '['};
    private static char[] _closingBrackets = { ')', '}', ']' };

    public const string Name = "Bracket";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("Parentheses", OnParenthesesMenuItemClick),
                new("Curly", OnCurlyMenuItemClick),
                new("Square", OnSquareMenuItemClick),
                new("-"),
                new("Trim Once", OnTrimOnceMenuItemClick),
                new("Trim All", OnTrimAllMenuItemClick)
            ]
        );

    private static void OnParenthesesMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), AddParentheses);
    private static void OnCurlyMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), AddCurly);
    private static void OnSquareMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), AddSquare);
    private static void OnTrimOnceMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), TrimOnce);
    private static void OnTrimAllMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToEachLine(ToolHelper.GetNoteTextBoxFromSender(sender), TrimAll);

    private static string? AddParentheses(string line, int index)
        => $"({line})";

    private static string? AddCurly(string line, int index)
        =>  $"{{{line}}}";

    private static string? AddSquare(string line, int index)
        => $"[{line}]";

    private static string? TrimOnce(string line, int index)
    {
        if (line.Length > 0 && _openingBrackets.Contains(line[0]) && _closingBrackets.Contains(line[^1]))
            return line[1..^1];

        return line;
    }

    private static string? TrimAll(string line, int index)
    {
        string newLine = line;

        while (newLine.Length > 0 && _openingBrackets.Contains(newLine[0]) && _closingBrackets.Contains(newLine[^1]))
            newLine = newLine[1..^1];

        return newLine;
    }
}
