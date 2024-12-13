using System;
using System.Linq;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Tools;

public partial class QuoteTool : BaseTool, ITool
{
    public ToolStates State { get; }

    private static char[] _openingQuotes = { '\'', '"', '`', '‘', '“' };
    private static char[] _closingQuotes = { '\'', '"', '`', '’', '”' };

    public enum ToolActions
    {
        QuoteDouble,
        QuoteSingle,
        Backtick,
        Trim
    }

    public QuoteTool(NoteTextBoxControl noteTextBox, ToolStates state) : base(noteTextBox)
    {
        State = state;
        _name = "Quote";
        _menuActions.Add(new("Double", QuoteDoubleMenuAction));
        _menuActions.Add(new("Single", QuoteSingleMenuAction));
        _menuActions.Add(new("Backtick", BacktickMenuAction));
        _menuActions.Add(new("-"));
        _menuActions.Add(new("Trim", TrimMenuAction));
    }

    private void QuoteDoubleMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.QuoteDouble);
    private void QuoteSingleMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.QuoteSingle);
    private void BacktickMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.Backtick);
    private void TrimMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.Trim);

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
