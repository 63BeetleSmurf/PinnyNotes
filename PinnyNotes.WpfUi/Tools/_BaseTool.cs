using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Controls;
using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.Tools;

public abstract class BaseTool
{
    protected SettingsService _settings;

    protected NoteTextBoxControl _noteTextBox;

    private MenuItem? _menuItem;
    public MenuItem MenuItem => _menuItem
        ?? throw new Exception("Tools menu has not been initialised.");

    public BaseTool(NoteTextBoxControl noteTextBox)
    {
        _settings = App.Services.GetRequiredService<SettingsService>();
        _noteTextBox = noteTextBox;
    }

    protected void InitializeMenuItem(string header, ToolMenuAction[] menuActions)
    {
        _menuItem = new()
        {
            Header = header,
        };

        foreach (ToolMenuAction menuAction in menuActions)
        {
            if (menuAction.Name == "-" && menuAction.Command == null && menuAction.Action == null)
            {
                _menuItem.Items.Add(new Separator());
                continue;
            }

            MenuItem actionMenuItem = new()
            {
                Header = menuAction.Name
            };
            if (menuAction.Command != null)
                actionMenuItem.Command = menuAction.Command;
            if (menuAction.Action != null)
                actionMenuItem.CommandParameter = menuAction.Action;

            _menuItem.Items.Add(actionMenuItem);
        }
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
            _noteTextBox.SelectAll();
            _noteTextBox.SelectedText = function(noteText, action);
            _noteTextBox.SelectionLength = 0;
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
            _noteTextBox.SelectAll();
            _noteTextBox.SelectedText = noteText;
            _noteTextBox.SelectionLength = 0;
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
        if (!hasSelectedText && _noteTextBox.KeepNewLineAtEndVisible && caretAtEnd)
            _noteTextBox.ScrollToEnd();
    }
}
