using System;
using System.Linq;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public class QuoteTool : BaseTool, ITool
{
    public ToolStates State => (ToolStates)Settings.Default.QuoteToolState;

    private static char[] _openingQuotes = { '\'', '"', '`', '‘', '“' };
    private static char[] _closingQuotes = { '\'', '"', '`', '’', '”' };

    public enum ToolActions
    {
        QuoteDouble,
        QuoteSingle,
        Backtick,
        Trim
    }

    public QuoteTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "Quote";
        _menuActions.Add(new("Double", new RelayCommand(() => MenuAction(ToolActions.QuoteDouble))));
        _menuActions.Add(new("Single", new RelayCommand(() => MenuAction(ToolActions.QuoteSingle))));
        _menuActions.Add(new("Backtick", new RelayCommand(() => MenuAction(ToolActions.Backtick))));
        _menuActions.Add(new("-"));
        _menuActions.Add(new("Trim", new RelayCommand(() => MenuAction(ToolActions.Trim))));
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
