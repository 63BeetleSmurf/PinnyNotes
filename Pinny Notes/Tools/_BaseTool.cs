using System;
using System.Collections.Generic;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Tools;

public abstract class BaseTool(NoteTextBoxControl noteTextBox)
{
    protected NoteTextBoxControl _noteTextBox = noteTextBox;
    protected string _name = null!;
    protected List<ToolMenuAction> _menuActions = [];

    public MenuItem GetMenuItem()
    {
        MenuItem menuItem = new()
        {
            Header = _name,
        };

        foreach (ToolMenuAction menuAction in _menuActions)
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

    protected void ApplyFunctionToNoteText(Func<string, Enum, string> function, Enum action)
    {
        if (_noteTextBox.SelectionLength > 0)
        {
            _noteTextBox.SelectedText = function(_noteTextBox.SelectedText, action);
        }
        else
        {
            string noteText = _noteTextBox.Text;
            // Ignore trailing new line if it was automatically added
            if (_noteTextBox.NewLineAtEnd && _noteTextBox.Text.EndsWith(Environment.NewLine))
                noteText = noteText.Remove(noteText.Length - Environment.NewLine.Length);
            _noteTextBox.Text = function(noteText, action);
            if (_noteTextBox.Text.Length > 0)
                _noteTextBox.CaretIndex = _noteTextBox.Text.Length - 1;
        }
    }

    protected void ApplyFunctionToEachLine(Func<string, int, Enum, string?> function, Enum action)
    {
        bool hasSelectedText = (_noteTextBox.SelectionLength > 0);
        string noteText = (hasSelectedText) ? _noteTextBox.SelectedText : _noteTextBox.Text;

        string[] lines = noteText.Split(Environment.NewLine);
        // Ignore trailing new line if it was automatically added
        if (_noteTextBox.NewLineAtEnd && lines[^1] == "")
            lines = lines[..^1];

        List<string> newLines = [];
        for (int i = 0; i < lines.Length; i++)
        {
            string? line = function(lines[i], i, action);
            if (line != null)
                newLines.Add(line);
        }

        noteText = string.Join(Environment.NewLine, newLines);

        if (hasSelectedText)
            _noteTextBox.SelectedText = noteText;
        else
        {
            _noteTextBox.Text = noteText;
            if (_noteTextBox.Text.Length > 0)
                _noteTextBox.CaretIndex = _noteTextBox.Text.Length - 1;
        }
    }

    protected void InsertIntoNoteText(string text)
    {
        bool hasSelectedText = (_noteTextBox.SelectionLength > 0);
        int caretIndex = (hasSelectedText) ? _noteTextBox.SelectionStart : _noteTextBox.CaretIndex;
        bool caretAtEnd = (caretIndex == _noteTextBox.Text.Length);

        _noteTextBox.SelectedText = text;

        _noteTextBox.CaretIndex = caretIndex + text.Length;
        if (!hasSelectedText && _noteTextBox.KeepNewLineVisible && caretAtEnd)
            _noteTextBox.ScrollToEnd();
    }
}
