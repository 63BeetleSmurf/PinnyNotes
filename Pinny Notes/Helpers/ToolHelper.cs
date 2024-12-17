using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Tools;
using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Helpers;

public static class ToolHelper
{
    public static MenuItem GetToolMenuItem(string toolName, IEnumerable<ToolMenuAction> menuActions)
    {
        MenuItem menuItem = new()
        {
            Header = toolName,
        };

        foreach (ToolMenuAction menuAction in menuActions)
        {
            if (menuAction.Name == "-" && menuAction.EventHandler == null)
            {
                menuItem.Items.Add(new Separator());
                continue;
            }

            MenuItem actionMenuItem = new()
            {
                Header = menuAction.Name
            };
            if (menuAction.EventHandler != null)
                actionMenuItem.Click += menuAction.EventHandler;

            menuItem.Items.Add(actionMenuItem);
        }

        return menuItem;
    }

    public static NoteTextBoxControl GetNoteTextBoxFromSender(object sender)
    {
        MenuItem? clickedMenuItem = sender as MenuItem;

        DependencyObject? currentItem = clickedMenuItem;
        while (currentItem != null && currentItem is not ContextMenu)
        {
            if (currentItem is MenuItem menuItem)
                currentItem = menuItem.Parent;
            else
                throw new Exception("Error traversing up to find ContextMenu.");
        }

        ContextMenu? contextMenu = currentItem as ContextMenu;

        NoteTextBoxControl? noteTextBox = contextMenu?.PlacementTarget as NoteTextBoxControl;
        if (noteTextBox == null)
            throw new Exception("Error getting target text box from ContextMenu.");

        return noteTextBox;
    }

    public static void ApplyFunctionToNoteText(NoteTextBoxControl noteTextBox, Func<string, object?, string> function, object? additionalParam = null)
    {
        if (noteTextBox.SelectionLength > 0)
        {
            noteTextBox.SelectedText = function(noteTextBox.SelectedText, additionalParam);
        }
        else
        {
            string noteText = noteTextBox.Text;
            // Ignore trailing new line if it was automatically added
            if (noteTextBox.NewLineAtEnd && noteTextBox.Text.EndsWith(Environment.NewLine))
                noteText = noteText.Remove(noteText.Length - Environment.NewLine.Length);
            noteTextBox.Text = function(noteText, additionalParam);
            if (noteTextBox.Text.Length > 0)
                noteTextBox.CaretIndex = noteTextBox.Text.Length - 1;
        }
    }

    public static void ApplyFunctionToEachLine(NoteTextBoxControl noteTextBox, Func<string, int, string?> function)
    {
        bool hasSelectedText = (noteTextBox.SelectionLength > 0);
        string noteText = (hasSelectedText) ? noteTextBox.SelectedText : noteTextBox.Text;

        string[] lines = noteText.Split(Environment.NewLine);
        // Ignore trailing new line if it was automatically added
        if (noteTextBox.NewLineAtEnd && lines[^1] == "")
            lines = lines[..^1];

        List<string> newLines = [];
        for (int i = 0; i < lines.Length; i++)
        {
            string? line = function(lines[i], i);
            if (line != null)
                newLines.Add(line);
        }

        noteText = string.Join(Environment.NewLine, newLines);

        if (hasSelectedText)
            noteTextBox.SelectedText = noteText;
        else
        {
            noteTextBox.Text = noteText;
            if (noteTextBox.Text.Length > 0)
                noteTextBox.CaretIndex = noteTextBox.Text.Length - 1;
        }
    }

    public static void InsertIntoNoteText(NoteTextBoxControl noteTextBox, string text)
    {
        bool hasSelectedText = (noteTextBox.SelectionLength > 0);
        int caretIndex = (hasSelectedText) ? noteTextBox.SelectionStart : noteTextBox.CaretIndex;
        bool caretAtEnd = (caretIndex == noteTextBox.Text.Length);

        noteTextBox.SelectedText = text;

        noteTextBox.CaretIndex = caretIndex + text.Length;
        if (!hasSelectedText && noteTextBox.KeepNewLineVisible && caretAtEnd)
            noteTextBox.ScrollToEnd();
    }
}
