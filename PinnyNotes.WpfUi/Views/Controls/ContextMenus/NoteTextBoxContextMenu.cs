using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Tools;
using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Views.Controls.ContextMenus;

public class NoteTextBoxContextMenu : ContextMenu
{
    private readonly NoteTextBoxControl _noteTextBox;

    public NoteTextBoxContextMenu(NoteTextBoxControl noteTextBox)
    {
        _noteTextBox = noteTextBox;

        PopulateMenu();
    }

    public void PopulateMenu()
    {
        AddSpellCheckMenuItems();


        MenuItem copyMenuItem = new()
        {
            Header = "Copy",
            InputGestureText = "Ctrl+C",
            IsEnabled = (_noteTextBox.SelectionLength > 0)
        };
        copyMenuItem.Click += OnCopyClicked;
        Items.Add(copyMenuItem);

        MenuItem cutMenuItem = new()
        {
            Header = "Cut",
            InputGestureText = "Ctrl+X",
            IsEnabled = (_noteTextBox.SelectionLength > 0)
        };
        cutMenuItem.Click += OnCutClicked;
        Items.Add(cutMenuItem);

        MenuItem pasteMenuItem = new()
        {
            Header = "Paste",
            InputGestureText = "Ctrl+V",
            IsEnabled = Clipboard.ContainsText()
        };
        pasteMenuItem.Click += OnPasteClicked;
        Items.Add(pasteMenuItem);


        Items.Add(new Separator());

        MenuItem selectAllMenuItem = new()
        {
            Header = "Select All",
            IsEnabled = (_noteTextBox.Text.Length > 0)
        };
        selectAllMenuItem.Click += OnSelectAllClicked;
        Items.Add(selectAllMenuItem);

        MenuItem clearMenuItem = new()
        {
            Header = "Clear",
            IsEnabled = (_noteTextBox.Text.Length > 0)
        };
        clearMenuItem.Click += OnClearClicked;
        Items.Add(clearMenuItem);


        Items.Add(new Separator());


        MenuItem countsMenuItem = new()
        {
            Header = "Counts"
        };
        countsMenuItem.Items.Add(
            new MenuItem()
            {
                Header = $"Lines: {_noteTextBox.GetLineCount()}",
                IsEnabled = false
            }
        );
        countsMenuItem.Items.Add(
            new MenuItem()
            {
                Header = $"Words: {_noteTextBox.GetWordCount()}",
                IsEnabled = false
            }
        );
        countsMenuItem.Items.Add(
            new MenuItem()
            {
                Header = $"Chars: {_noteTextBox.GetCharCount()}",
                IsEnabled = false
            }
        );
        Items.Add(countsMenuItem);


        AddToolContextMenus();
    }

    private void OnCopyClicked(object sender, RoutedEventArgs e)
        => _noteTextBox.Copy();

    private void OnCutClicked(object sender, RoutedEventArgs e)
        => _noteTextBox.Cut();

    private void OnPasteClicked(object sender, RoutedEventArgs e)
        => _noteTextBox.Paste();

    private void OnSelectAllClicked(object sender, RoutedEventArgs e)
        => _noteTextBox.SelectAll();

    private void OnClearClicked(object sender, RoutedEventArgs e)
        => _noteTextBox.Clear();

    private void AddSpellCheckMenuItems()
    {
        SpellingError spellingError = _noteTextBox.GetSpellingError(_noteTextBox.CaretIndex);
        if (spellingError == null)
            return;

        if (!spellingError.Suggestions.Any())
            Items.Add(
                new MenuItem()
                {
                    Header = "(no spelling suggestions)",
                    IsEnabled = false
                }
            );
        else
            foreach (string spellingSuggestion in spellingError.Suggestions)
                Items.Add(
                    new MenuItem()
                    {
                        Header = spellingSuggestion,
                        FontWeight = FontWeights.Bold,
                        Command = EditingCommands.CorrectSpellingError,
                        CommandParameter = spellingSuggestion,
                        CommandTarget = _noteTextBox
                    }
                );

        Items.Add(new Separator());
    }

    private void AddToolContextMenus()
    {
        MenuItem toolsMenu = new()
        {
            Header = "Tools"
        };

        AddToolToMenu(Base64Tool.MenuItem, _noteTextBox.Tool_Base64State, toolsMenu.Items);
        AddToolToMenu(BracketTool.MenuItem, _noteTextBox.Tool_BracketState, toolsMenu.Items);
        AddToolToMenu(CaseTool.MenuItem, _noteTextBox.Tool_CaseState, toolsMenu.Items);
        AddToolToMenu(DateTimeTool.MenuItem, _noteTextBox.Tool_DateTimeState, toolsMenu.Items);
        AddToolToMenu(GibberishTool.MenuItem, _noteTextBox.Tool_GibberishState, toolsMenu.Items);
        AddToolToMenu(HashTool.MenuItem, _noteTextBox.Tool_HashState, toolsMenu.Items);
        AddToolToMenu(HtmlEntityTool.MenuItem, _noteTextBox.Tool_HtmlEntityState, toolsMenu.Items);
        AddToolToMenu(IndentTool.MenuItem, _noteTextBox.Tool_IndentState, toolsMenu.Items);
        AddToolToMenu(JoinTool.MenuItem, _noteTextBox.Tool_JoinState, toolsMenu.Items);
        AddToolToMenu(JsonTool.MenuItem, _noteTextBox.Tool_JsonState, toolsMenu.Items);
        AddToolToMenu(ListTool.MenuItem, _noteTextBox.Tool_ListState, toolsMenu.Items);
        AddToolToMenu(QuoteTool.MenuItem, _noteTextBox.Tool_QuoteState, toolsMenu.Items);
        AddToolToMenu(RemoveTool.MenuItem, _noteTextBox.Tool_RemoveState, toolsMenu.Items);
        AddToolToMenu(SlashTool.MenuItem, _noteTextBox.Tool_SlashState, toolsMenu.Items);
        AddToolToMenu(SortTool.MenuItem, _noteTextBox.Tool_SortState, toolsMenu.Items);
        AddToolToMenu(SplitTool.MenuItem, _noteTextBox.Tool_SplitState, toolsMenu.Items);
        AddToolToMenu(TrimTool.MenuItem, _noteTextBox.Tool_TrimState, toolsMenu.Items);

        if (toolsMenu.Items.Count > 0)
            Items.Add(toolsMenu);
    }

    private void AddToolToMenu(MenuItem toolMenuItem, ToolStates toolState, ItemCollection toolMenuItems)
    {
        switch (toolState)
        {
            case ToolStates.Favorite:
                Items.Add(toolMenuItem);
                break;
            case ToolStates.Enabled:
                toolMenuItems.Add(toolMenuItem);
                break;
        }
    }
}
