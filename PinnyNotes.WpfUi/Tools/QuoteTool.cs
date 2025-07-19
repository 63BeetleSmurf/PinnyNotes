using System;
using System.Linq;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class QuoteTool : BaseTool, ITool
{
    private enum ToolActions
    {
        QuoteDouble,
        QuoteSingle,
        Backtick,
        Trim
    }

    private readonly char[] _openingQuotes = ['\'', '"', '`', '‘', '“'];
    private readonly char[] _closingQuotes = ['\'', '"', '`', '’', '”'];

    public ToolStates State => ToolSettings.QuoteToolState;

    public QuoteTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Quote",
            [
                new ToolMenuAction("Double", new RelayCommand(() => MenuAction(ToolActions.QuoteDouble))),
                new ToolMenuAction("Single", new RelayCommand(() => MenuAction(ToolActions.QuoteSingle))),
                new ToolMenuAction("Backtick", new RelayCommand(() => MenuAction(ToolActions.Backtick))),
                new ToolMenuAction("-"),
                new ToolMenuAction("Trim", new RelayCommand(() => MenuAction(ToolActions.Trim)))
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
            case ToolActions.QuoteDouble:
                return $"\"{line}\"";
            case ToolActions.QuoteSingle:
                return $"'{line}'";
            case ToolActions.Backtick:
                return $"`{line}`";
            case ToolActions.Trim:
            {
                if (line.Length > 0 && _openingQuotes.Contains(line[0]) && _closingQuotes.Contains(line[^1]))
                    return line[1..^1];
                else
                    return line;
            }
        }

        return line;
    }
}
