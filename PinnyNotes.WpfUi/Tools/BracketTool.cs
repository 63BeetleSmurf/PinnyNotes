using System;
using System.Linq;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class BracketTool : BaseTool, ITool
{
    private enum ToolActions
    {
        BracketParentheses,
        BracketCurly,
        BracketSquare,
        BracketTrimOnce,
        BracketTrimAll,
    }

    private readonly char[] _openingBrackets = ['(', '{', '['];
    private readonly char[] _closingBrackets = [')', '}', ']'];

    public ToolStates State => ToolSettings.BracketToolState;

    public BracketTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Bracket",
            [
                new ToolMenuAction("Parentheses", new RelayCommand(() => MenuAction(ToolActions.BracketParentheses))),
                new ToolMenuAction("Curly", new RelayCommand(() => MenuAction(ToolActions.BracketCurly))),
                new ToolMenuAction("Square", new RelayCommand(() => MenuAction(ToolActions.BracketSquare))),
                new ToolMenuAction("-"),
                new ToolMenuAction("Trim Once", new RelayCommand(() => MenuAction(ToolActions.BracketTrimOnce))),
                new ToolMenuAction("Trim All", new RelayCommand(() => MenuAction(ToolActions.BracketTrimAll)))
            ]
        );
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
