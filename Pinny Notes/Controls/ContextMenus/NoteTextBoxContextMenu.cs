using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using PinnyNotes.WpfUi.Tools;

namespace PinnyNotes.WpfUi.Controls.ContextMenus;

public class NoteTextBoxContextMenu : ContextMenu
{
    private NoteTextBoxControl _noteTextBox;

    private ITool[] _tools;

    private List<Control> _spellingErrorMenuItems = [];
    private MenuItem _copyMenuItem;
    private MenuItem _cutMenuItem;
    private MenuItem _pasteMenuItem;
    private MenuItem _selectAllMenuItem;
    private MenuItem _clearMenuItem;
    private MenuItem _lineCountMenuItem;
    private MenuItem _wordCountMenuItem;
    private MenuItem _charCountMenuItem;

    public NoteTextBoxContextMenu(NoteTextBoxControl noteTextBox)
    {
        _noteTextBox = noteTextBox;

        _tools = [
            new Base64Tool(_noteTextBox),
            new BracketTool(_noteTextBox),
            new CaseTool(_noteTextBox),
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

        Populate();
    }

    public void Update()
    {
        bool hasText = (_noteTextBox.Text.Length > 0);
        bool hasSelectedText = (_noteTextBox.SelectionLength > 0);

        UpdateSpellingErrorMenuItems();

        _copyMenuItem.IsEnabled = hasSelectedText;
        _cutMenuItem.IsEnabled = hasSelectedText;
        _pasteMenuItem.IsEnabled = Clipboard.ContainsText();

        _selectAllMenuItem.IsEnabled = hasText;
        _clearMenuItem.IsEnabled = hasText;

        _lineCountMenuItem.Header = $"Lines: {_noteTextBox.LineCount()}";
        _wordCountMenuItem.Header = $"Words: {_noteTextBox.WordCount()}";
        _charCountMenuItem.Header = $"Chars: {_noteTextBox.CharCount()}";
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

        MenuItem countsMenuItem = new()
        {
            Header = "Counts"
        };
        countsMenuItem.Items.Add(_lineCountMenuItem);
        countsMenuItem.Items.Add(_wordCountMenuItem);
        countsMenuItem.Items.Add(_charCountMenuItem);
        Items.Add(countsMenuItem);

        AddToolContextMenus();
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

    private void AddToolContextMenus()
    {
        IEnumerable<ITool> favouriteTools = _tools.Where(t => t.IsEnabled && t.IsFavourite);
        bool hasFavouriteTools = favouriteTools.Any();
        IEnumerable<ITool> standardTools = _tools.Where(t => t.IsEnabled && !t.IsFavourite);
        bool hasStandardTools = standardTools.Any();

        if (hasFavouriteTools || hasStandardTools)
            Items.Add(new Separator());

        foreach (ITool tool in favouriteTools)
        {
            if (tool.IsEnabled)
                Items.Add(
                    tool.GetMenuItem()
                );
        }

        if (hasStandardTools)
        {
            MenuItem toolsMenu = new()
            {
                Header = "Tools"
            };
            foreach (ITool tool in standardTools)
            {
                toolsMenu.Items.Add(
                    tool.GetMenuItem()
                );
            }
            Items.Add(toolsMenu);
        }
    }

}
