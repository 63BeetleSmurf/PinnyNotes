using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Tools;

namespace PinnyNotes.WpfUi.Controls.ContextMenus;

public class NoteTextBoxContextMenu : ContextMenu
{
    private readonly NoteTextBoxControl _noteTextBox;

    private readonly ITool[] _tools;

    private readonly MenuItem _copyMenuItem;
    private readonly MenuItem _cutMenuItem;
    private readonly MenuItem _pasteMenuItem;
    private readonly MenuItem _selectAllMenuItem;
    private readonly MenuItem _clearMenuItem;
    private readonly MenuItem _countsMenuItem;
    private readonly MenuItem _lineCountMenuItem;
    private readonly MenuItem _wordCountMenuItem;
    private readonly MenuItem _charCountMenuItem;
    private readonly Separator _toolsSeparator = new();
    private readonly MenuItem _toolsMenuItem;

    private readonly List<Control> _spellingErrorMenuItems = [];
    private readonly List<Control> _toolMenuItems = [];

    public NoteTextBoxContextMenu(NoteTextBoxControl noteTextBox)
    {
        _noteTextBox = noteTextBox;

        _tools = [
            new Base64Tool(_noteTextBox),
            new BracketTool(_noteTextBox),
            new CaseTool(_noteTextBox),
            new ColorTool(_noteTextBox),
            new DateTimeTool(_noteTextBox),
            new GibberishTool(_noteTextBox),
            new HashTool(_noteTextBox),
            new HtmlEntityTool(_noteTextBox),
            new IndentTool(_noteTextBox),
            new JoinTool(_noteTextBox),
            new JsonTool(_noteTextBox),
            new ListTool(_noteTextBox),
            new QuoteTool(_noteTextBox),
            new RemoveTool(_noteTextBox),
            new SlashTool(_noteTextBox),
            new SortTool(_noteTextBox),
            new SplitTool(_noteTextBox),
            new TrimTool(_noteTextBox)
        ];

        _copyMenuItem = new()
        {
            Header = "Copy",
            Command = _noteTextBox.CopyCommand,
            InputGestureText = "Ctrl+C"
        };
        _cutMenuItem = new()
        {
            Header = "Cut",
            Command = _noteTextBox.CutCommand,
            InputGestureText = "Ctrl+X"
        };
        _pasteMenuItem = new()
        {
            Header = "Paste",
            Command = _noteTextBox.PasteCommand,
            InputGestureText = "Ctrl+V"
        };

        _selectAllMenuItem = new()
        {
            Header = "Select All",
            Command = new RelayCommand(_noteTextBox.SelectAll)
        };
        _clearMenuItem = new()
        {
            Header = "Clear",
            Command = _noteTextBox.ClearCommand
        };

        _countsMenuItem = new()
        {
            Header = "Counts"
        };
        _lineCountMenuItem = new()
        {
            IsEnabled = false
        };
        _wordCountMenuItem = new()
        {
            IsEnabled = false
        };
        _charCountMenuItem = new()
        {
            IsEnabled = false
        };

        _toolsMenuItem = new()
        {
            Header = "Tools"
        };

        Populate();
    }

    public void Update()
    {
        bool hasText = (_noteTextBox.Text.Length > 0);

        UpdateSpellingErrorMenuItems();

        _copyMenuItem.IsEnabled = _noteTextBox.HasSelectedText;
        _cutMenuItem.IsEnabled = _noteTextBox.HasSelectedText;
        _pasteMenuItem.IsEnabled = Clipboard.ContainsText();

        _selectAllMenuItem.IsEnabled = hasText;
        _clearMenuItem.IsEnabled = hasText;

        _lineCountMenuItem.Header = $"Lines: {_noteTextBox.LineCount()}";
        _wordCountMenuItem.Header = $"Words: {_noteTextBox.WordCount()}";
        _charCountMenuItem.Header = $"Chars: {_noteTextBox.CharCount()}";

        UpdateToolContextMenus();
    }

    private void Populate()
    {
        Items.Add(_copyMenuItem);
        Items.Add(_cutMenuItem);
        Items.Add(_pasteMenuItem);

        Items.Add(new Separator());

        Items.Add(_selectAllMenuItem);
        Items.Add(_clearMenuItem);

        Items.Add(new Separator());

        _countsMenuItem.Items.Add(_lineCountMenuItem);
        _countsMenuItem.Items.Add(_wordCountMenuItem);
        _countsMenuItem.Items.Add(_charCountMenuItem);
        Items.Add(_countsMenuItem);
    }

    private void UpdateSpellingErrorMenuItems()
    {
        foreach (Control spellingErrorMenuItem in _spellingErrorMenuItems)
            Items.Remove(spellingErrorMenuItem);

        _spellingErrorMenuItems.Clear();

        int caretIndex = _noteTextBox.CaretIndex;
        SpellingError spellingError = _noteTextBox.GetSpellingError(caretIndex);
        if (spellingError != null)
        {
            if (!spellingError.Suggestions.Any())
                _spellingErrorMenuItems.Add(
                    new MenuItem()
                    {
                        Header = "(no spelling suggestions)",
                        IsEnabled = false
                    }
                );
            else
                foreach (string spellingSuggestion in spellingError.Suggestions)
                {
                    _spellingErrorMenuItems.Add(
                        new MenuItem()
                        {
                            Header = spellingSuggestion,
                            FontWeight = FontWeights.Bold,
                            Command = EditingCommands.CorrectSpellingError,
                            CommandParameter = spellingSuggestion,
                            CommandTarget = _noteTextBox
                        }
                    );
                }
            _spellingErrorMenuItems.Add(new Separator());
        }

        _spellingErrorMenuItems.Reverse();
        foreach (Control spellingErrorMenuItem in _spellingErrorMenuItems)
            Items.Insert(0, spellingErrorMenuItem);
    }

    private void UpdateToolContextMenus()
    {
        foreach (Control toolMenuItem in _toolMenuItems)
            Items.Remove(toolMenuItem);
        _toolsMenuItem.Items.Clear();

        _toolMenuItems.Clear();

        IEnumerable<ITool> activeTools = _tools.Where(t => t.State != ToolStates.Disabled);
        if (!activeTools.Any())
            return;

        _toolMenuItems.Add(_toolsSeparator);

        bool hasEnabledTools = false;
        foreach (ITool tool in activeTools)
        {
            switch (tool.State)
            {
                case ToolStates.Favourite:
                    _toolMenuItems.Add(tool.MenuItem);
                    break;
                case ToolStates.Enabled:
                    _toolsMenuItem.Items.Add(tool.MenuItem);
                    hasEnabledTools = true;
                    break;
            }
        }

        if (hasEnabledTools)
            _toolMenuItems.Add(_toolsMenuItem);

        foreach (Control toolMenuItem in _toolMenuItems)
            Items.Add(toolMenuItem);
    }
}
