using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;

using PinnyNotes.WpfUi.Tools;

namespace PinnyNotes.WpfUi.Views.Controls;

public class NoteContextMenu : ContextMenu
{
    private readonly NoteTextBox _noteTextBox;

    private void OnCopyClicked(object sender, RoutedEventArgs e) => _noteTextBox.Copy();
    private void OnCutClicked(object sender, RoutedEventArgs e) => _noteTextBox.Cut();
    private void OnPasteClicked(object sender, RoutedEventArgs e) => _noteTextBox.Paste();
    private void OnSelectAllClicked(object sender, RoutedEventArgs e) => _noteTextBox.SelectAll();
    private void OnClearClicked(object sender, RoutedEventArgs e) => _noteTextBox.Clear();

    public NoteContextMenu(NoteTextBox noteTextBox)
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
        IEnumerable<ITool> favouriteTools = _noteTextBox.Tools.Where(t => t.IsEnabled && t.IsFavourite);
        bool hasFavouriteTools = favouriteTools.Any();
        IEnumerable<ITool> standardTools = _noteTextBox.Tools.Where(t => t.IsEnabled && !t.IsFavourite);
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
