using System;
using System.Linq;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Tools;

public partial class BracketTool : BaseTool, ITool
{
    public ToolStates State => (ToolStates)ToolSettings.Default.BracketToolState;

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

    public BracketTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        _name = "Bracket";
        _menuActions.Add(new("Parentheses", BracketParenthesesMenuAction));
        _menuActions.Add(new("Curly", BracketCurlyMenuAction));
        _menuActions.Add(new("Square", BracketSquareMenuAction));
        _menuActions.Add(new("-"));
        _menuActions.Add(new("Trim Once", BracketTrimOnceMenuAction));
        _menuActions.Add(new("Trim All", BracketTrimAllMenuAction));
    }

    private void BracketParenthesesMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.BracketParentheses);
    private void BracketCurlyMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.BracketCurly);
    private void BracketSquareMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.BracketSquare);
    private void BracketTrimOnceMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.BracketTrimOnce);
    private void BracketTrimAllMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.BracketTrimAll);

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
