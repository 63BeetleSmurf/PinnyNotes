using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;
using System.Windows.Controls;

using Pinny_Notes.Properties;

namespace Pinny_Notes.Tools;

public partial class QuoteTool : BaseTool, ITool
{
    public bool IsEnabled => ToolSettings.Default.QuoteToolEnabled;
    public bool IsFavourite => ToolSettings.Default.QuoteToolFavourite;

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
        _menuActions.Add(new("Double", MenuActionCommand, ToolActions.QuoteDouble));
        _menuActions.Add(new("Single", MenuActionCommand, ToolActions.QuoteSingle));
        _menuActions.Add(new("Backtick", MenuActionCommand, ToolActions.Backtick));
        _menuActions.Add(new("-"));
        _menuActions.Add(new("Trim", MenuActionCommand, ToolActions.Trim));
    }

    [RelayCommand]
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
