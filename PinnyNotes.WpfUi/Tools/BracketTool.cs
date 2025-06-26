using System;
using System.Linq;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public class BracketTool : BaseTool, ITool
{
    public bool IsEnabled => ToolSettings.Default.BracketToolEnabled;
    public bool IsFavourite => ToolSettings.Default.BracketToolFavourite;

    private static char[] _openingBrackets = {'(', '{', '['};
    private static char[] _closingBrackets = { ')', '}', ']' };

    public enum ToolActions
    {
        BracketParentheses,
        BracketCurly,
        BracketSquare,
        BracketTrimOnce,
        BracketTrimAll,
    }

    public BracketTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "Bracket";
        _menuActions.Add(new("Parentheses", new RelayCommand(() => MenuAction(ToolActions.BracketParentheses))));
        _menuActions.Add(new("Curly", new RelayCommand(() => MenuAction(ToolActions.BracketCurly))));
        _menuActions.Add(new("Square", new RelayCommand(() => MenuAction(ToolActions.BracketSquare))));
        _menuActions.Add(new("-"));
        _menuActions.Add(new("Trim Once", new RelayCommand(() => MenuAction(ToolActions.BracketTrimOnce))));
        _menuActions.Add(new("Trim All", new RelayCommand(() => MenuAction(ToolActions.BracketTrimAll))));
    }

    private void MenuAction(ToolActions action)
    {
        ApplyFunctionToEachLine(ModifyLineCallback, action);
    }

    private string? ModifyLineCallback(string line, int index, Enum action)
    {
        switch (action)
        {
            case ToolActions.BracketParentheses:
                return $"({line})";
            case ToolActions.BracketCurly:
                return $"{{{line}}}";
            case ToolActions.BracketSquare:
                return $"[{line}]";
            case ToolActions.BracketTrimOnce:
            {
                if (line.Length > 0 && _openingBrackets.Contains(line[0]) && _closingBrackets.Contains(line[^1]))
                    return line[1..^1];
                else
                    return line;
            }
            case ToolActions.BracketTrimAll:
            {
                string newLine = line;
                while (newLine.Length > 0 && _openingBrackets.Contains(newLine[0]) && _closingBrackets.Contains(newLine[^1]))
                    newLine = newLine[1..^1];
                return newLine;
            }
        }

        return line;
    }
}
